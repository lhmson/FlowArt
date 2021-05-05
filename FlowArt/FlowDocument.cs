using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Northwoods.Go;
using Northwoods.Go.Xml;

namespace FlowArt
{
    /// <summary>
    /// Summary description for FlowDocument.
    /// </summary>
    [Serializable]
    public class FlowDocument : GoDocument
    {
        public FlowDocument()
        {
            this.Name = "FlowArt Design";
            this.IsModified = true;

            MaintainsPartID = true; //added to test unique ID
        }

        public String Location
        {
            get { return myLocation; }
            set
            {
                String old = myLocation;
                if (old != value)
                {
                    RemoveDocument(old);
                    myLocation = value;
                    AddDocument(value, this);
                    RaiseChanged(ChangedLocation, 0, null, 0, old, NullRect, 0, value, NullRect);
                }
            }
        }

        public void InsertComment()
        {
            GoComment comment = new GoComment();
            comment.Text = "comment sth";
            comment.Position = NextNodePosition();
            comment.Label.Multiline = true;
            comment.Label.Editable = true;
            StartTransaction();
            Add(comment);
            FinishTransaction("Insert Comment");
        }

        public void InsertNode(BlockType k)
        {
            GoObject n = new FlowBlock(k);
            n.Position = NextNodePosition();
            StartTransaction();
            Add(n);
            FinishTransaction("Insert Block");
        }


        public PointF NextNodePosition()
        {
            PointF next = myNextNodePos;
            myNextNodePos.X += 20;
            if (myNextNodePos.X > 600)
            {
                myNextNodePos.X = 200;
                myNextNodePos.Y += 20;
                if (myNextNodePos.Y > 300)
                    myNextNodePos.Y = 200;
            }
            return next;
        }

        public override void ChangeValue(GoChangedEventArgs e, bool undo)
        {
            switch (e.Hint)
            {
                case ChangedLocation:
                    {
                        this.Location = (String)e.GetValue(undo);
                        break;
                    }
                default:
                    base.ChangeValue(e, undo);
                    return;
            }
        }

        private PointF InternalNextNodePos
        {
            get { return myNextNodePos; }
            set { myNextNodePos = value; }
        }

        public int Version
        {
            get { return 3; }
            set
            {
                if (value != this.Version)
                    throw new NotSupportedException("No support for different versions of saved design");
            }
        }

        // adapt the XML elements and attributes to match your classes and their properties
        private static void InitReaderWriter(GoXmlReaderWriterBase rw)
        {
            GoXmlBindingTransformer.DefaultTracingEnabled = true;  // for debugging, check your Output window (trace listener)
            GoXmlBindingTransformer t;

            t = new GoXmlBindingTransformer("flowchart", new FlowDocument());
            t.AddBinding("version", "Version", GoXmlBindingFlags.RethrowsExceptions);  // let exception from Version setter propagate out
            t.AddBinding("name", "Name");
            t.AddBinding("nextnodepos", "InternalNextNodePos");
            rw.AddTransformer(t);

            t = new GoXmlBindingTransformer("node", new FlowBlock(BlockType.Process));
            t.IdAttributeUsedForSharedObjects = true;  // each FlowBlock gets a unique ID
            t.HandlesNamedPorts = true;  // generate attributes for each of the named ports, specifying their IDs
            t.AddBinding("kind", "Kind");
            t.AddBinding("text", "Text");
            t.AddBinding("pos", "Position");
            rw.AddTransformer(t);

            t = new GoXmlBindingTransformer("title", new Title());
            rw.AddTransformer(t);

            t = new GoXmlBindingTransformer("comment", new GoComment());
            t.IdAttributeUsedForSharedObjects = true;  // each GoComment gets a unique ID
            t.AddBinding("text", "Text");
            t.AddBinding("familyname", "Label.FamilyName");
            t.AddBinding("fontsize", "Label.FontSize");
            t.AddBinding("alignment", "Label.Alignment");
            t.AddBinding("bold", "Label.Bold");
            t.AddBinding("italic", "Label.Italic");
            t.AddBinding("strikethrough", "Label.StrikeThrough");
            t.AddBinding("underline", "Label.Underline");
            t.AddBinding("multiline", "Label.Multiline");
            t.AddBinding("wrapping", "Label.Wrapping");
            t.AddBinding("wrappingwidth", "Label.WrappingWidth");
            t.AddBinding("editable", "Label.Editable");
            t.AddBinding("loc", "Location");  // last property, since it depends on content/alignment
            rw.AddTransformer(t);

            t = new GoXmlBindingTransformer("link", new GraphLink());
            t.AddBinding("from", "FromPort");
            t.AddBinding("to", "ToPort");
            t.AddBinding("label", "Text");
            rw.AddTransformer(t);
        }


        public void Store(Stream file, String loc)
        {
            bool oldskips = this.SkipsUndoManager;
            this.SkipsUndoManager = true;
            this.Location = loc;
            int lastslash = loc.LastIndexOf("\\");
            if (lastslash >= 0)
                this.Name = loc.Substring(lastslash + 1);
            else
                this.Name = loc;
            this.IsModified = false;
            this.SkipsUndoManager = oldskips;

            GoXmlWriter xw = new GoXmlWriter();
            InitReaderWriter(xw);
            xw.NodesGeneratedFirst = true;
            xw.Objects = this;
            xw.Generate(file);
        }

        public static FlowDocument Load(Stream file, String loc)
        {
            GoXmlReader xr = new GoXmlReader();
            InitReaderWriter(xr);
            FlowDocument doc = xr.Consume(file) as FlowDocument;
            if (doc == null) return null;

            // update the file location
            doc.Location = loc;
            // undo managers are not serialized
            doc.UndoManager = new GoUndoManager();
            doc.IsModified = false;
            AddDocument(loc, doc);
            return doc;
        }

        public override bool IsReadOnly
        {
            get
            {
                if (this.Location == "") return false;
                FileInfo info = new FileInfo(this.Location);
                bool ro = ((info.Attributes & FileAttributes.ReadOnly) != 0);
                bool oldskips = this.SkipsUndoManager;
                this.SkipsUndoManager = true;
                // take out the following statement if you want the user to be able
                // to modify the design even though the file is read-only
                SetModifiable(!ro);
                this.SkipsUndoManager = oldskips;
                return ro;
            }
        }


        public static int NextDocumentID()
        {
            return myDocCounter++;
        }

        public static FlowDocument FindDocument(String location)
        {
            return myDocuments[location] as FlowDocument;
        }

        internal static void AddDocument(String location, FlowDocument doc)
        {
            myDocuments[location] = doc;
        }

        internal static void RemoveDocument(String location)
        {
            myDocuments.Remove(location);
        }


        public const int ChangedLocation = LastHint + 23;

        private static int myDocCounter = 1;
        private static Hashtable myDocuments = new Hashtable();

        private String myLocation = "";
        private PointF myNextNodePos = new PointF(200, 200);
    }

    [Serializable]
    public class Title : GoComment
    {
        public Title()
        {
            this.Label.Multiline = true;
            this.Label.Editable = false;
            this.Label.Alignment = GoObject.MiddleTop;
        }

        protected override GoObject CreateBackground()
        {
            return null;
        }
    }

    [Serializable]
    public class GraphLink : GoLabeledLink
    {
        public GraphLink()
        {
            this.Orthogonal = true;
            this.Style = GoStrokeStyle.RoundedLineWithJumpGaps;
            this.ToArrow = true;
            this.Pen = new Pen(Color.DarkGray, 4);
            this.BrushColor = Color.DarkGray;
            this.ToArrowLength = 12;
            this.ToArrowShaftLength = 12;
            this.ToArrowWidth = 15;
        }

        public String Text
        {  // null value means no FromLabel
            get
            {
                GoText lab = this.FromLabel as GoText;
                if (lab != null)
                    return lab.Text;
                else
                    return null;
            }
            set
            {
                if (value == null)
                {
                    this.FromLabel = null;
                }
                else
                {
                    GoText lab = this.FromLabel as GoText;
                    if (lab == null)
                    {
                        lab = new GoText();
                        lab.Selectable = false;
                        lab.Editable = true;
                        this.FromLabel = lab;
                    }
                    lab.Text = value;
                }
            }
        }
    }
}
