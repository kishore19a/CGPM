﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;
using System.Windows.Forms;
using System.Speech.Recognition;

namespace CGPMFinal
{
    public partial class Form2 : Form
    {
        SpeechRecognitionEngine speechRecognitionEngine = null;
        List<Word> words = new List<Word>();
        public Form2()
        {
            InitializeComponent();
            try
            {
                // create the engine
                speechRecognitionEngine = createSpeechEngine("en-US");

                // hook to events
                speechRecognitionEngine.AudioLevelUpdated += new EventHandler<AudioLevelUpdatedEventArgs>(engine_AudioLevelUpdated);
                speechRecognitionEngine.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(engine_SpeechRecognized);

                // load dictionary
                loadGrammarAndCommands();

                // use the system's default microphone
                speechRecognitionEngine.SetInputToDefaultAudioDevice();

                // start listening
                speechRecognitionEngine.RecognizeAsync(RecognizeMode.Multiple);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Voice recognition failed");
            }
        }

        private void loadGrammarAndCommands()
        {
            try
            {
                Choices texts = new Choices();
                string[] lines = File.ReadAllLines("E:\\_Acad_7th_Sem\\ME735 Computer Graphics and Product Modeling\\CGPMFinal1\\CGPMFinal\\CGPMFinal\\bin\\Debug\\example.txt");
                foreach (string line in lines)
                {
                    // skip commentblocks and empty lines..
                    if (line.StartsWith("--") || line == String.Empty) continue;

                    // split the line
                    var parts = line.Split(new char[] { '|' });

                    // add commandItem to the list for later lookup or execution
                    words.Add(new Word() { Text = parts[0], AttachedText = parts[1], IsShellCommand = (parts[2] == "true") });

                    // add the text to the known choices of speechengine
                    texts.Add(parts[0]);
                }
                Grammar wordsList = new Grammar(new GrammarBuilder(texts));
                speechRecognitionEngine.LoadGrammar(wordsList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void engine_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            txtSpoken.Text += "\r" + getKnownTextOrExecute(e.Result.Text);
            string temp = " ";
            if (getKnownTextOrExecute(e.Result.Text) != "<")
            {
                temp = getKnownTextOrExecute(e.Result.Text);
            }

            if (getKnownTextOrExecute(e.Result.Text) == ">")
            {
                string sub1 = txtSpoken.Text.Substring(0, txtSpoken.Text.Length - 2);
                txtSpoken.Text = sub1 + "\r\n";
                ///txtSpoken.Text = txtSpoken.Text + "\r\n" ;
            }
            if (getKnownTextOrExecute(e.Result.Text) == "xx")
            {
                txtSpoken.Text = "";
            }
            if (getKnownTextOrExecute(e.Result.Text) == "<")
            {
                string sub = txtSpoken.Text.Substring(0, txtSpoken.Text.Length - 3);
                ///string sub = txtSpoken.Text.Replace("<", string.Empty).Replace(temp,string.Empty);
                txtSpoken.Text = sub;
            }
            string T = txtSpoken.Text;
            File.WriteAllText("E:\\_Acad_7th_Sem\\ME735 Computer Graphics and Product Modeling\\CGPMFinal1\\CGPMFinal\\Output.txt", T);
            ///temp = getKnownTextOrExecute(e.Result.Text);
            //scvText.ScrollToEnd();
        }

        private string getKnownTextOrExecute(string command)
        {
            try
            {
                var cmd = words.Where(c => c.Text == command).First();

                if (cmd.IsShellCommand)
                {
                    Process proc = new Process();
                    proc.EnableRaisingEvents = false;
                    proc.StartInfo.FileName = cmd.AttachedText;
                    proc.Start();
                    return "you just started : " + cmd.AttachedText;
                }
                else
                {

                    return cmd.AttachedText;
                }
            }
            catch (Exception)
            {
                return command;
            }
        }

        private void engine_AudioLevelUpdated(object sender, AudioLevelUpdatedEventArgs e)
        {
            prgLevel.Value = e.AudioLevel;
        }

        private SpeechRecognitionEngine createSpeechEngine(string preferredCulture)
        {
            foreach (RecognizerInfo config in SpeechRecognitionEngine.InstalledRecognizers())
            {
                if (config.Culture.ToString() == preferredCulture)
               { 
                    speechRecognitionEngine = new SpeechRecognitionEngine(config);
                    break;
                }
            }

            // if the desired culture is not found, then load default
            if (speechRecognitionEngine == null)
            {
                MessageBox.Show("The desired culture is not installed on this machine, the speech-engine will continue using "
                    + SpeechRecognitionEngine.InstalledRecognizers()[0].Culture.ToString() + " as the default culture.",
                    "Culture " + preferredCulture + " not found!");
                speechRecognitionEngine = new SpeechRecognitionEngine(SpeechRecognitionEngine.InstalledRecognizers()[0]);
            }

            return speechRecognitionEngine;
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            // unhook events
            speechRecognitionEngine.RecognizeAsyncStop();
            // clean references
            speechRecognitionEngine.Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtSpoken_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
