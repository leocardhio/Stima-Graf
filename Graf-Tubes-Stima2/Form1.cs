using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Graf_Tubes_Stima2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                Title = "Browse Input Files",
                DefaultExt = "txt",
                RestoreDirectory = true
            };

            textBox1.IsAccessible = false;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
            }
            renderComboBoxes(sender, e);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {
            
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.ShowDialog();
            openFileDialog1.InitialDirectory = @"C:\";
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.Title = "Browse Input File";
            openFileDialog1.DefaultExt = "txt";
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        { 
            string[] testspek = File.ReadAllLines(textBox1.Text);
            List<string> filenodes = BacaFile.getNodes(testspek);
            foreach (string node in filenodes)
            {
                if (comboBox1.Items.Contains(node) || comboBox2.Text==node)
                {
                    // do nothing
                } else
                {
                    comboBox1.Items.Add(node);
                }
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string[] testspek = File.ReadAllLines(textBox1.Text);
            List<string> filenodes = BacaFile.getNodes(testspek);
            foreach (string node in filenodes)
            {
                if (comboBox2.Items.Contains(node) || comboBox1.Text==node)
                {
                    //Do nothing
                } else
                {
                    comboBox2.Items.Add(node);
                }
            }
        }

        private void renderComboBoxes(object sender, EventArgs e)
        {
            comboBox1_SelectedIndexChanged(sender, e);
            comboBox2_SelectedIndexChanged(sender, e);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text != "" && (radioButton1.Checked || radioButton2.Checked))
            {
                //////create a viewer object 
                Microsoft.Msagl.GraphViewerGdi.GViewer viewer = new Microsoft.Msagl.GraphViewerGdi.GViewer();
                //////create a graph object 
                Microsoft.Msagl.Drawing.Graph graph = new Microsoft.Msagl.Drawing.Graph("graph");
                //////create the graph content 
                if (radioButton1.Checked)
                {
                    graph = DFSHandler();
                }
                else if (radioButton2.Checked)
                {
                    graph = BFSHandler();
                }

                graph.Directed = false;
                viewer.IsAccessible = false;
                viewer.ToolBarIsVisible = false;
                //////bind the graph to the viewer 
                viewer.Graph = graph;
                //////associate the viewer with the form 

                pictureBox1.SuspendLayout();
                viewer.Dock = System.Windows.Forms.DockStyle.Fill;

                pictureBox1.Controls.Clear();
                pictureBox1.Controls.Add(viewer);
                viewer.FitGraphBoundingBox();
                pictureBox1.ResumeLayout();

                Label label13 = new Label();
                Size sLabel13 = new Size(250, 20);
                Point pLabel13 = new Point(36, 766);
                label13.Size = sLabel13;
                label13.Location = pLabel13;
                label13.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                label13.Text = "Friends Recommendation for " + comboBox1.Text + " :";
                Controls.Add(label13);
                handleMutualFriends(sender, e, graph);
                button2.Hide();
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            

        }
        private Microsoft.Msagl.Drawing.Graph DFSHandler()
        {
            //////create a graph object 
            string[] testspek = File.ReadAllLines(textBox1.Text);
            List<string> filenodes = BacaFile.getNodes(testspek);
            bool[,] adjMatrix = BacaFile.getAdjMatrix(testspek, filenodes);
            List<string> DFSpath = DFS.FindPathDFS(filenodes, adjMatrix, comboBox1.Text, comboBox2.Text);
            Microsoft.Msagl.Drawing.Graph graph = new Microsoft.Msagl.Drawing.Graph("graph");
            graph = getPlainGraph(DFSpath);
            //////create a viewer object 
            Microsoft.Msagl.GraphViewerGdi.GViewer viewer = new Microsoft.Msagl.GraphViewerGdi.GViewer();
           
            for (int i = 0; i < DFSpath.Count - 1; i++)
            {
                Microsoft.Msagl.Drawing.Node source = graph.FindNode(DFSpath[i]);
                Microsoft.Msagl.Drawing.Node target = graph.FindNode(DFSpath[i+1]);
                source.Attr.FillColor = Microsoft.Msagl.Drawing.Color.GreenYellow;
                var edge = graph.AddEdge(DFSpath[i], DFSpath[i + 1]);
                edge.Attr.Color = Microsoft.Msagl.Drawing.Color.GreenYellow;
                edge.Attr.ArrowheadAtTarget = Microsoft.Msagl.Drawing.ArrowStyle.None;
                
                if (i == DFSpath.Count - 2)
                {
                    target.Attr.FillColor = Microsoft.Msagl.Drawing.Color.GreenYellow;
                }
            }
            return graph;
        }

        private Microsoft.Msagl.Drawing.Graph BFSHandler()
        {
            string[] testspek = File.ReadAllLines(textBox1.Text);
            List<string> filenodes = BacaFile.getNodes(testspek);
            bool[,] adjMatrix = BacaFile.getAdjMatrix(testspek, filenodes);
            List<string> BFSpath = BFS.FindPathBFS(filenodes, adjMatrix, comboBox1.Text, comboBox2.Text);
            Microsoft.Msagl.Drawing.Graph graph = new Microsoft.Msagl.Drawing.Graph("graph");
            graph = getPlainGraph(BFSpath);
            Microsoft.Msagl.GraphViewerGdi.GViewer viewer = new Microsoft.Msagl.GraphViewerGdi.GViewer();
            for (int i = 0; i < BFSpath.Count - 1; i++)
            {
                Microsoft.Msagl.Drawing.Node source = graph.FindNode(BFSpath[i]);
                Microsoft.Msagl.Drawing.Node target = graph.FindNode(BFSpath[i + 1]);
                source.Attr.FillColor = Microsoft.Msagl.Drawing.Color.GreenYellow;
                var edge = graph.AddEdge(BFSpath[i], BFSpath[i + 1]);
                edge.Attr.Color = Microsoft.Msagl.Drawing.Color.GreenYellow;
                edge.Attr.ArrowheadAtTarget = Microsoft.Msagl.Drawing.ArrowStyle.None;

                if (i == BFSpath.Count - 2)
                {
                    target.Attr.FillColor = Microsoft.Msagl.Drawing.Color.GreenYellow;
                }
            }
            return graph;
        }

        private Microsoft.Msagl.Drawing.Graph getPlainGraph(List<string> DFSpath)
        {
            string[] testspek = File.ReadAllLines(textBox1.Text);
            List<string> filenodes = BacaFile.getNodes(testspek);
            bool[,] adjMatrix = BacaFile.getAdjMatrix(testspek, filenodes);

            Microsoft.Msagl.Drawing.Graph graph2 = new Microsoft.Msagl.Drawing.Graph("graph");
            int totalNode = filenodes.Count;
            int totalAdj = adjMatrix.Length;
            int idx = 0;
            int i; int j;

            while (idx < totalAdj)
            {
                i = idx / totalNode;
                j = idx % totalNode;

                if (adjMatrix[i, j])
                {
                    if (!isValidDFSPath(filenodes[i], filenodes[j], DFSpath))
                    {
                        graph2.AddEdge(filenodes[i], filenodes[j]).Attr.ArrowheadAtTarget = Microsoft.Msagl.Drawing.ArrowStyle.None;
                    }
                    adjMatrix[j, i] = false;
                }
                idx++;
            }
            return graph2;
        }

        private void handleMutualFriends(object sender, EventArgs e, Microsoft.Msagl.Drawing.Graph graph)
        {
            string[] testspek = File.ReadAllLines(textBox1.Text);
            List<string> filenodes = BacaFile.getNodes(testspek);
            bool[,] adjMatrix = BacaFile.getAdjMatrix(testspek, filenodes);

            Dictionary<string, List<string>> mutuals = DFS.MutualFriendsDFS(filenodes, adjMatrix, comboBox1.Text);
            
            createMutualVisual(sender, e, mutuals,graph);
        }

        private void createMutualVisual (object sender, EventArgs e, Dictionary<string, List<string>> mutual, Microsoft.Msagl.Drawing.Graph graph)
        {
            Point initPRadio = new Point(36, 805);
            Point initPText = new Point(56,832);
            foreach (KeyValuePair<string, List<string>> entry in mutual)
            {
                if (entry.Value.Count > 0)
                {
                    RadioButton rMutual = new RadioButton();
                    TextBox tMutual = new TextBox();
                    Size stMutual = new Size(238, 20);
                    rMutual.Text = entry.Key;
                    rMutual.Font = new Font("Segoe UI", 9);
                    tMutual.Text = entry.Value.Count.ToString() + " Mutual Friends: ";
                    tMutual.Font = new Font("Segoe UI", 9);
                    tMutual.BorderStyle = BorderStyle.None;
                    tMutual.IsAccessible = false;
                    tMutual.Size = stMutual;


                    for (int i = 0; i < entry.Value.Count; i++) 
                    {
                        tMutual.Text = tMutual.Text + entry.Value[i];
                        if (i != entry.Value.Count - 1)
                        {
                            tMutual.Text = tMutual.Text + ",";
                        }
                    }

                    rMutual.Click += new System.EventHandler((sender,e) => rMutualGraphDrawer(sender,e,rMutual,graph));

                    Controls.Add(rMutual);
                    Controls.Add(tMutual);
                    rMutual.Location = initPRadio;
                    tMutual.Location = initPText;
                    initPText.Y += 60;
                    initPRadio.Y += 60;
                }
            }
            Label blankSpace = new Label();
            blankSpace.Location = initPRadio;
            Controls.Add(blankSpace);
        }

        private void rMutualGraphDrawer(object sender, EventArgs e, RadioButton rbutton, Microsoft.Msagl.Drawing.Graph graph)
        {
            Microsoft.Msagl.GraphViewerGdi.GViewer viewer = new Microsoft.Msagl.GraphViewerGdi.GViewer();
            graph.FindNode(rbutton.Text).Attr.FillColor = Microsoft.Msagl.Drawing.Color.Green;
            pictureBox1.Controls.Clear();
            viewer.IsAccessible = false;
            viewer.ToolBarIsVisible = false;
            viewer.Graph = graph;
            pictureBox1.Controls.Add(viewer);
            viewer.FitGraphBoundingBox();
            pictureBox1.ResumeLayout();
        }

        private bool isValidDFSPath(string node1, string node2, List<string> DFSpath)
        {
            bool isValid = false;
            int i = 0;
            while (!isValid && i < DFSpath.Count - 1)
            {
                if ((DFSpath[i] == node1 && DFSpath[i + 1] == node2) || (DFSpath[i]==node2 && DFSpath[i+1]==node1))
                {
                    
                    isValid = true;
                }
                else
                {
                    i++;
                }
            }

            return isValid;
        }
    }
}
