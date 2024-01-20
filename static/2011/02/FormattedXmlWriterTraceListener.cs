using System;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Collections;
using System.Xml.XPath;

namespace System.Diagnostics
{
    public class FormattedXmlWriterTraceListener : XmlWriterTraceListener
    {
        #region Fields
        
        private XmlTextWriter xmlWriter;
        private Process currentProcess;
        private TextWriter writer;
        private Stream stream;
        private string fileName;
        private object writerLock = new object();
        private string machineName;
        private int indentation = 2; 

        #endregion

        #region Constructors
    
        /// <summary>
        /// Initializes a new instance of the FormattedXmlWriterTraceListener class, using the specified stream as the recipient of the debugging and tracing output.
        /// </summary>
        /// <param name="stream">A System.IO.Stream that represents the stream the trace listener writes to.</param>
        public FormattedXmlWriterTraceListener(Stream stream)
            : base(string.Empty)
        {
            this.stream = stream;
            this.Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the FormattedXmlWriterTraceListener class, using the specified file as the recipient of the debugging and tracing output.
        /// </summary>
        /// <param name="filename">The name of the file to write to.</param>
        public FormattedXmlWriterTraceListener(string filename)
            : base(string.Empty)
        {
            this.fileName = filename;
            this.Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the FormattedXmlWriterTraceListener class, using the specified writer as the recipient of the debugging and tracing output.
        /// </summary>
        /// <param name="writer">A System.IO.TextWriter that receives the output from the trace listener.</param>
        public FormattedXmlWriterTraceListener(TextWriter writer)
            : base(string.Empty)
        {
            this.writer = writer;
            this.Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the FormattedXmlWriterTraceListener class with the specified name, using the specified stream as the recipient of the debugging and tracing output.
        /// </summary>
        /// <param name="stream">A System.IO.Stream that represents the stream the trace listener writes to.</param>
        /// <param name="name">The name of the new instance.</param>
        public FormattedXmlWriterTraceListener(Stream stream, string name)
            : base(string.Empty, name)
        {
            this.stream = stream;
            this.Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the FormattedXmlWriterTraceListener class with the specified name, using the specified file as the recipient of the debugging and tracing output.
        /// </summary>
        /// <param name="filename">The name of the file to write to.</param>
        /// <param name="name">The name of the new instance.</param>
        public FormattedXmlWriterTraceListener(string filename, string name)
            : base(string.Empty, name)
        {
            this.fileName = filename;
            this.Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the FormattedXmlWriterTraceListener class with the specified name, using the specified writer as the recipient of the debugging and tracing output.
        /// </summary>
        /// <param name="writer">A System.IO.TextWriter that receives the output from the trace listener.</param>
        /// <param name="name">The name of the new instance.</param>
        public FormattedXmlWriterTraceListener(TextWriter writer, string name)
            : base(string.Empty, name)
        {
            this.writer = writer;
            this.Initialize();
        } 

        #endregion

        #region Public Properties

        /// <summary>
        /// The XmlTextWriter used to write the debugging and tracing output.
        /// </summary>
        public XmlTextWriter XmlWriter
        {
            get { return this.xmlWriter; }
        }

        /// <summary>
        /// Indicates how the output is formatted.
        /// </summary>
        /// <value>
        /// One of the System.Xml.Formatting values. The default is Formatting.None (no special formatting).
        /// </value>
        public Formatting Formatting { get; set; }

        /// <summary>
        /// Gets or sets how many IndentChars to write for each level in the hierarchy when System.Xml.XmlTextWriter.Formatting is set to Formatting.Indented.
        /// </summary>
        /// <value>
        /// Number of IndentChars for each level. The default is 2.
        /// </value>
        /// <exception cref="System.ArgumentException">Setting this property to a negative value.</exception>
        public int Indentation
        {
            get { return indentation; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Indentation value must be greater than 0.");
                indentation = value;
            }
        }

        /// <summary>
        /// Gets or sets which character to use for indenting when System.Xml.XmlTextWriter.Formatting is set to Formatting.Indented.
        /// </summary>
        /// <value>
        /// The character to use for indenting. The default is space.NoteThe XmlTextWriter allows you to set this property to any character.
        /// To ensure valid XML, you must specify a valid white space character, 0x9, 0x10, 0x13 or 0x20.
        /// </value>
        public char IndentChar { get; set; }

        #endregion

        #region Protected Methods
        /// <summary>
        /// Disposes the QueuedXmlWriterTraceListener 
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (this.xmlWriter != null)
                this.xmlWriter.Close();
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Flushes the output buffer for the writer.
        /// </summary>
        public override void Flush()
        {
            if (this.xmlWriter != null)
                this.xmlWriter.Flush();
        }

        /// <summary>
        /// Flushes the listener then closes the writer for this listener so it no longer receives tracing or debugging output.
        /// </summary>
        public override void Close()
        {
            this.Flush();
            if (this.xmlWriter != null)
                this.xmlWriter.Close();
            this.xmlWriter = null;
        }

        /// <summary>
        /// Writes trace information, a message, and event information to the file or stream.
        /// </summary>
        /// <param name="eventCache">A System.Diagnostics.TraceEventCache that contains the current process ID, thread ID, and stack trace information.</param>
        /// <param name="source">The source name.</param>
        /// <param name="eventType">One of the System.Diagnostics.TraceEventType values.</param>
        /// <param name="id">A numeric identifier for the event.</param>
        /// <param name="message">The message to write.</param>
        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
        {
            if ((base.Filter == null) || base.Filter.ShouldTrace(eventCache, source, eventType, id, message, null, null, null))
            {
                lock (writerLock)
                {
                    if (this.EnsureXmlWriter())
                    {
                        this.WriteHeader(source, eventType, id, eventCache);
                        this.XmlWriter.WriteValue(message);
                        this.WriteFooter(eventCache);
                    }
                }
            }
        }

        /// <summary>
        /// Writes trace information, a message, and event information to the file or stream.
        /// </summary>
        /// <param name="eventCache">A System.Diagnostics.TraceEventCache that contains the current process ID, thread ID, and stack trace information.</param>
        /// <param name="source">The source name.</param>
        /// <param name="eventType">One of the System.Diagnostics.TraceEventType values.</param>
        /// <param name="id">A numeric identifier for the event.</param>
        /// <param name="format">A format string that contains zero or more format items that correspond to objects in the args array.</param>
        /// <param name="args">An object array containing zero or more objects to format.</param>
        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string format, params object[] args)
        {
            this.TraceEvent(eventCache, source, eventType, id, string.Format(format, args));
        }

        /// <summary>
        /// Writes trace information, a message, and event information to the file or stream.
        /// </summary>
        /// <param name="eventCache">A System.Diagnostics.TraceEventCache that contains the current process ID, thread ID, and stack trace information.</param>
        /// <param name="source">The source name.</param>
        /// <param name="eventType">One of the System.Diagnostics.TraceEventType values.</param>
        /// <param name="id">A numeric identifier for the event.</param>
        /// <param name="data">A data object to emit.</param>
        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
        {
            this.TraceData(eventCache, source, eventType, id, new object[] { data });
        }

        /// <summary>
        /// Writes trace information, a message, and event information to the file or stream.
        /// </summary>
        /// <param name="eventCache">A System.Diagnostics.TraceEventCache that contains the current process ID, thread ID, and stack trace information.</param>
        /// <param name="source">The source name.</param>
        /// <param name="eventType">One of the System.Diagnostics.TraceEventType values.</param>
        /// <param name="id">A numeric identifier for the event.</param>
        /// <param name="data">An array of data objects to emit.</param>
        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, params object[] data)
        {
            if ((base.Filter == null) || base.Filter.ShouldTrace(eventCache, source, eventType, id, null, null, null, data))
            {
                lock (writerLock)
                {
                    if (EnsureXmlWriter())
                    {
                        WriteHeader(source, eventType, id, eventCache);
                        XmlWriter.WriteStartElement("TraceData");
                        if (data != null)
                        {
                            for (int i = 0; i < data.Length; i++)
                            {
                                XmlWriter.WriteStartElement("DataItem");
                                if (data[i] != null)
                                {
                                    WriteData(data[i]);
                                }
                                XmlWriter.WriteEndElement();
                            }
                        }
                        XmlWriter.WriteEndElement();
                        WriteFooter(eventCache);
                    }
                }
            }
        }

        /// <summary>
        /// Writes trace information including the identity of a related activity, a message, and event information to the file or stream.
        /// </summary>
        /// <param name="eventCache">A System.Diagnostics.TraceEventCache that contains the current process ID, thread ID, and stack trace information.</param>
        /// <param name="source">The source name.</param>
        /// <param name="id">A numeric identifier for the event.</param>
        /// <param name="message">A trace message to write.</param>
        /// <param name="relatedActivityId">A System.Guid structure that identifies a related activity.</param>
        public override void TraceTransfer(TraceEventCache eventCache, string source, int id, string message, Guid relatedActivityId)
        {
            lock (writerLock)
            {
                if (this.EnsureXmlWriter())
                {
                    this.WriteHeader(source, TraceEventType.Transfer, id, eventCache, relatedActivityId);
                    this.XmlWriter.WriteValue(message);
                    this.WriteFooter(eventCache);
                }
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        private void Initialize()
        {
            machineName = Environment.MachineName;
            this.IndentChar = ' ';
        }

        /// <summary>
        /// Sets the XML writer.
        /// </summary>
        /// <param name="writer">The writer.</param>
        private void SetXmlWriter(XmlTextWriter writer)
        {
            if (writer == null) throw new ArgumentNullException("writer");
            this.xmlWriter = writer;
            this.xmlWriter.Formatting = this.Formatting;
            this.xmlWriter.Indentation = this.Indentation;
            this.xmlWriter.IndentChar = this.IndentChar;
        }

        /// <summary>
        /// Ensures the writer is created.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if the writer is created, otherwise <c>false</c>.
        /// </returns>
        private bool EnsureWriter()
        {
            bool flag = true;
            if (this.writer == null)
            {
                flag = false;
                if (this.fileName == null)
                {
                    return flag;
                }
                Encoding encodingWithFallback = GetEncodingWithFallback(new UTF8Encoding(false));
                string fullPath = Path.GetFullPath(this.fileName);
                string directoryName = Path.GetDirectoryName(fullPath);
                string fileName = Path.GetFileName(fullPath);
                for (int i = 0; i < 2; i++)
                {
                    try
                    {
                        this.writer = new StreamWriter(fullPath, true, encodingWithFallback, 0x1000);
                        flag = true;
                        break;
                    }
                    catch (IOException)
                    {
                        fileName = Guid.NewGuid().ToString() + fileName;
                        fullPath = Path.Combine(directoryName, fileName);
                    }
                    catch (UnauthorizedAccessException)
                    {
                        break;
                    }
                    catch (Exception)
                    {
                        break;
                    }
                }
                if (!flag)
                {
                    this.fileName = null;
                }
            }
            return flag;
        }

        /// <summary>
        /// Writes the end header.
        /// </summary>
        /// <param name="eventCache">The event cache.</param>
        private void WriteEndHeader(TraceEventCache eventCache)
        {
            if (currentProcess == null)
                currentProcess = Process.GetCurrentProcess();

            this.XmlWriter.WriteStartElement("Execution");
            this.XmlWriter.WriteAttributeString("ProcessName", currentProcess.ProcessName);
            this.XmlWriter.WriteAttributeString("ProcessID", ((uint)currentProcess.Id).ToString(CultureInfo.InvariantCulture));
            this.XmlWriter.WriteStartAttribute("ThreadID");
            if (eventCache != null)
            {
                this.XmlWriter.WriteValue(eventCache.ThreadId.ToString(CultureInfo.InvariantCulture));
            }
            else
            {
                this.XmlWriter.WriteValue(Thread.CurrentThread.ManagedThreadId.ToString(CultureInfo.InvariantCulture));
            }
            this.XmlWriter.WriteEndAttribute();
            this.XmlWriter.WriteEndElement();

            this.XmlWriter.WriteElementString("Channel", null);
            this.XmlWriter.WriteStartElement("Computer");
            this.XmlWriter.WriteValue(machineName);
            this.XmlWriter.WriteEndElement();
            this.XmlWriter.WriteEndElement();
            this.XmlWriter.WriteStartElement("ApplicationData");
        }

        /// <summary>
        /// Writes the start header.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="eventType">Type of the event.</param>
        /// <param name="id">The id.</param>
        /// <param name="eventCache">The event cache.</param>
        /// <param name="relatedActivityId">The related activity id.</param>
        private void WriteStartHeader(string source, TraceEventType eventType, int id, TraceEventCache eventCache, Guid? relatedActivityId)
        {
            this.XmlWriter.WriteStartElement("E2ETraceEvent");
            this.XmlWriter.WriteAttributeString("xmlns", "http://schemas.microsoft.com/2004/06/E2ETraceEvent");

            this.XmlWriter.WriteStartElement("System");
            this.XmlWriter.WriteAttributeString("xmlns", "http://schemas.microsoft.com/2004/06/windows/eventlog/system");

            this.XmlWriter.WriteStartElement("EventID");
            this.XmlWriter.WriteValue(((uint)id).ToString(CultureInfo.InvariantCulture));
            this.XmlWriter.WriteEndElement();

            this.XmlWriter.WriteStartElement("Type");
            this.XmlWriter.WriteValue("3");
            this.XmlWriter.WriteEndElement();

            this.XmlWriter.WriteStartElement("SubType");
            this.XmlWriter.WriteAttributeString("Name", eventType.ToString());
            this.XmlWriter.WriteValue(0);
            this.XmlWriter.WriteEndElement();

            this.XmlWriter.WriteStartElement("Level");
            int num = (int)eventType;
            if (num > 0xff)
            {
                num = 0xff;
            }
            if (num < 0)
            {
                num = 0;
            }
            this.XmlWriter.WriteValue(num.ToString(CultureInfo.InvariantCulture));
            this.XmlWriter.WriteEndElement();

            this.XmlWriter.WriteStartElement("TimeCreated");
            this.XmlWriter.WriteStartAttribute("SystemTime");
            if (eventCache != null)
            {
                this.XmlWriter.WriteValue(eventCache.DateTime.ToString("o", CultureInfo.InvariantCulture));
            }
            else
            {
                this.XmlWriter.WriteValue(DateTime.Now.ToString("o", CultureInfo.InvariantCulture));
            }
            this.XmlWriter.WriteEndAttribute();
            this.XmlWriter.WriteEndElement();

            this.XmlWriter.WriteStartElement("Source");
            this.XmlWriter.WriteAttributeString("Name", source);
            this.XmlWriter.WriteEndElement();

            this.XmlWriter.WriteStartElement("Correlation");
            this.XmlWriter.WriteStartAttribute("ActivityID");
            if (eventCache != null)
            {
                this.XmlWriter.WriteValue(Trace.CorrelationManager.ActivityId.ToString("B"));
            }
            else
            {
                this.XmlWriter.WriteValue(Guid.Empty.ToString("B"));
            }
            this.XmlWriter.WriteEndAttribute();
            if (relatedActivityId != null)
            {
                this.XmlWriter.WriteAttributeString("RelatedActivityID", relatedActivityId.Value.ToString("B"));
            }
            this.XmlWriter.WriteEndElement();
        }

        /// <summary>
        /// Determines whether the specified TraceOptions are enabled.
        /// </summary>
        /// <param name="opts">The TraceOptions.</param>
        /// <returns>
        ///   <c>true</c> if the specified TraceOptions is enabled; otherwise, <c>false</c>.
        /// </returns>
        private bool IsEnabled(TraceOptions opts)
        {
            return ((opts & this.TraceOutputOptions) != TraceOptions.None);
        }

        /// <summary>
        /// Writes the header.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="eventType">Type of the event.</param>
        /// <param name="id">The id.</param>
        /// <param name="eventCache">The event cache.</param>
        /// <param name="relatedActivityId">The related activity id.</param>
        private void WriteHeader(string source, TraceEventType eventType, int id, TraceEventCache eventCache, Guid relatedActivityId)
        {
            this.WriteStartHeader(source, eventType, id, eventCache, relatedActivityId);
            this.WriteEndHeader(eventCache);
        }

        /// <summary>
        /// Writes the header.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="eventType">Type of the event.</param>
        /// <param name="id">The id.</param>
        /// <param name="eventCache">The event cache.</param>
        private void WriteHeader(string source, TraceEventType eventType, int id, TraceEventCache eventCache)
        {
            this.WriteStartHeader(source, eventType, id, eventCache, null);
            this.WriteEndHeader(eventCache);
        }

        /// <summary>
        /// Writes the footer.
        /// </summary>
        /// <param name="eventCache">The event cache.</param>
        private void WriteFooter(TraceEventCache eventCache)
        {
            bool flag = this.IsEnabled(TraceOptions.LogicalOperationStack);
            bool flag2 = this.IsEnabled(TraceOptions.Callstack);
            if ((eventCache != null) && (flag || flag2))
            {
                this.XmlWriter.WriteStartElement("System.Diagnostics");
                this.XmlWriter.WriteAttributeString("xmlns", "http://schemas.microsoft.com/2004/08/System.Diagnostics");
                if (flag)
                {
                    this.XmlWriter.WriteStartElement("LogicalOperationStack");
                    Stack logicalOperationStack = eventCache.LogicalOperationStack;
                    if (logicalOperationStack != null)
                    {
                        foreach (object obj2 in logicalOperationStack)
                        {
                            this.XmlWriter.WriteStartElement("LogicalOperation");
                            this.XmlWriter.WriteValue(obj2.ToString());
                            this.XmlWriter.WriteEndElement();
                        }
                    }
                    this.XmlWriter.WriteEndElement();
                }
                this.XmlWriter.WriteStartElement("Timestamp");
                this.XmlWriter.WriteValue(eventCache.Timestamp.ToString(CultureInfo.InvariantCulture));
                this.XmlWriter.WriteEndElement();
                if (flag2)
                {
                    this.XmlWriter.WriteStartElement("Callstack");
                    this.XmlWriter.WriteValue(eventCache.Callstack);
                    this.XmlWriter.WriteEndElement();
                }
                this.XmlWriter.WriteEndElement();
            }
            this.XmlWriter.WriteEndElement();
            this.XmlWriter.WriteEndElement();
            this.writer.WriteLine();
        }

        /// <summary>
        /// Ensures the XML writer is created.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if the xml writer is created, otherwise <c>false</c>.
        /// </returns>
        private bool EnsureXmlWriter()
        {
            this.EnsureWriter();
            if (writer != null)
            {
                this.SetXmlWriter(new XmlTextWriter(this.writer));
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Gets the encoding with fallback.
        /// </summary>
        /// <param name="encoding">The encoding.</param>
        private static Encoding GetEncodingWithFallback(Encoding encoding)
        {
            Encoding encoding2 = (Encoding)encoding.Clone();
            encoding2.EncoderFallback = EncoderFallback.ReplacementFallback;
            encoding2.DecoderFallback = DecoderFallback.ReplacementFallback;
            return encoding2;
        }

        /// <summary>
        /// Writes the data.
        /// </summary>
        /// <param name="data">The data.</param>
        private void WriteData(object data)
        {
            XPathNavigator navigator = data as XPathNavigator;
            if (navigator == null)
            {
                this.XmlWriter.WriteValue(data.ToString());
            }
            else
            {
                try
                {
                    navigator.MoveToRoot();
                    this.XmlWriter.WriteNode(navigator, false);
                }
                catch (Exception)
                {
                    this.XmlWriter.WriteValue(data.ToString());
                }
            }
        }

        #endregion
    }
}
