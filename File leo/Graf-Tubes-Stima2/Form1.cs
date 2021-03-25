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
            TextBox textBox1 = new TextBox();
            textBox1.Text = openFileDialog1.FileName;
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
            //////create a viewer object 
            Microsoft.Msagl.GraphViewerGdi.GViewer viewer = new Microsoft.Msagl.GraphViewerGdi.GViewer();
            //////create a graph object 
            Microsoft.Msagl.Drawing.Graph graph = new Microsoft.Msagl.Drawing.Graph("graph");
            //////create the graph content 
            if (radioButton1.Checked)
            {
                graph = DFSHandler();
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
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            

        }
        private Microsoft.Msagl.Drawing.Graph DFSHandler()
        {
            Microsoft.Msagl.Drawing.Graph graph = new Microsoft.Msagl.Drawing.Graph("graph");
            graph = getPlainGraph();
            //////create a viewer object 
            Microsoft.Msagl.GraphViewerGdi.GViewer viewer = new Microsoft.Msagl.GraphViewerGdi.GViewer();
            //////create a graph object 
            string[] testspek = File.ReadAllLines(textBox1.Text);
            List<string> filenodes = BacaFile.getNodes(testspek);
            bool[,] adjMatrix = BacaFile.getAdjMatrix(testspek, filenodes);
            List<string> DFSpath = DFS.FindPathDFS(filenodes, adjMatrix, comboBox1.Text, comboBox2.Text);
            for (int i = 0; i < DFSpath.Count - 1; i++)
            {
                graph.FindNode(DFSpath[i]).Attr.FillColor = Microsoft.Msagl.Drawing.Color.GreenYellow;
                var edge = graph.AddEdge(DFSpath[i], DFSpath[i + 1]);
                edge.Attr.Color = Microsoft.Msagl.Drawing.Color.GreenYellow;
                edge.Attr.ArrowheadAtTarget = Microsoft.Msagl.Drawing.ArrowStyle.None;
                if (i == DFSpath.Count - 2)
                {
                    graph.FindNode(DFSpath[i+1]).Attr.FillColor = Microsoft.Msagl.Drawing.Color.GreenYellow;
                }
            }
            return graph;
        }

        private Microsoft.Msagl.Drawing.Graph getPlainGraph()
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

                if (adjMatrix[i,j]) 
                {
                    adjMatrix[j, i] = false;
                    graph2.AddEdge(filenodes[i], filenodes[j]).Attr.ArrowheadAtTarget = Microsoft.Msagl.Drawing.ArrowStyle.None;
                }
                idx++;
            }
            return graph2;
        }
    }
}
