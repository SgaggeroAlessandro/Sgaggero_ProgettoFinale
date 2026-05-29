    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    namespace Sgaggero_Snake
    {
        public partial class Form1 : Form
        {
            List<Point> snake = new List<Point>();
            Point cibo = new Point();
            int colonne = 20;
            int righe = 20;
            int punteggio = 0;
            bool fine = false;
            Direzione direzione;
            Direzione ultimaDirezione; //salva l'ultima direzione, per evitare errori quando si premono velocemente due tasti
            Random rnd = new Random(); 
        
        
            public enum Direzione
            {
                Destra,
                Sinistra,
                Su,
                Giu
            }


            public Form1()
            {
                InitializeComponent();

                

                    dataGridView1.DefaultCellStyle.SelectionBackColor = Color.Black;//nasconde l'indicatore blu che segnala lo spostamento del serpente tramite le frecce
                dataGridView1.ClearSelection();//serve a togliere il quadratino

                CreaGriglia();
                Start();
            }


            public void CreaGriglia()
            {


                dataGridView1.Columns.Clear();
                dataGridView1.Rows.Clear();

                for (int i = 0; i < colonne; i++)
                {
                    dataGridView1.Columns.Add("col" + i, "");
                    dataGridView1.Columns[i].Width = 20;
                }

                for (int j = 0; j < righe; j++)
                {
                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[j].Height = 20;
                }

                for (int r = 0; r < righe; r++)
                {
                    for (int c = 0; c < colonne; c++)
                    {
                        dataGridView1.Rows[r].Cells[c].Style.BackColor = Color.Black;
                    }
                }
            }

            public void Start()
            {

                snake.Clear();
                
                snake.Add(new Point(5, 1));
                snake.Add(new Point(4, 1));
                snake.Add(new Point(3, 1));
                direzione = Direzione.Destra;
                ultimaDirezione = Direzione.Destra;
                punteggio = 0;
                fine = false;
                labelPunteggio.Text = $"Punteggio: {punteggio}";

                timer1.Start();//avvia  l'intervallo automatico del timer
                GeneraCibo();
                ColoraSnake();
            

            }
            public void ColoraSnake()
            {
                for (int r = 0; r < righe; r++)
                {
                    for (int c = 0; c < colonne; c++)
                    {
                        dataGridView1.Rows[r].Cells[c].Style.BackColor = Color.Black;
                    }
                }
                for (int i = 0; i < snake.Count; i++)
                {
                    int X = snake[i].X;
                    int Y = snake[i].Y;
                    dataGridView1.Rows[Y].Cells[X].Style.BackColor = Color.Green;
                }


                        dataGridView1.Rows[cibo.Y].Cells[cibo.X].Style.BackColor = Color.Red;
                
                }
            public void GeneraCibo()
            {

                Point puntoCasuale = new Point();

                do
                {

                    int x = rnd.Next(0, colonne);
                   int y = rnd.Next(0, righe);

                    puntoCasuale = new Point(x, y);
                } while (snake.Contains(puntoCasuale));

                cibo = puntoCasuale;
            dataGridView1.Rows[cibo.Y].Cells[cibo.X].Style.BackColor = Color.Red;
        }

            private void timer1_Tick(object sender, EventArgs e)//ogni intervallo di tempo, deciso nel form1 progettazione, richiama la funzione MuoviSerpente 
            {
                MuoviSerpente();
            }

            private void Form1_KeyDown(object sender, KeyEventArgs e)
            {
                if ((e.KeyCode == Keys.W || e.KeyCode == Keys.Up) && ultimaDirezione != Direzione.Giu) // non permette di invertire la direzione
                {
                    direzione = Direzione.Su;
                }else if((e.KeyCode == Keys.A || e.KeyCode == Keys.Left) && ultimaDirezione != Direzione.Destra)
                {
                    direzione = Direzione.Sinistra;
                }
                else if ((e.KeyCode == Keys.S || e.KeyCode == Keys.Down) && ultimaDirezione != Direzione.Su)
                {
                    direzione = Direzione.Giu;
                }
                else if((e.KeyCode == Keys.D || e.KeyCode == Keys.Right) && ultimaDirezione != Direzione.Sinistra)
                {
                    direzione = Direzione.Destra;
                }
            }

            private void dataGridView1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)//evita che si usino i tasti freccia per spostarsi all'interno della griglia
            {
                if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Left || e.KeyCode == Keys.Right || e.KeyCode == Keys.W || e.KeyCode == Keys.A || e.KeyCode == Keys.S || e.KeyCode == Keys.D)
                {
                
                    e.IsInputKey = true;
                }
            }

            public void MuoviSerpente()
            {
            ultimaDirezione = direzione;
            Point testaAttuale = snake[0];
            Point nuovaTesta = testaAttuale; //poi verrà cambiato solo il valore interessato con l'if, l'altro rimarrà uguale alla "vecchia" testa
                if (direzione == Direzione.Su)
                {

                    nuovaTesta.Y = testaAttuale.Y - 1;
                   

                }
                else if (direzione == Direzione.Giu)
                {
                    nuovaTesta.Y = testaAttuale.Y + 1;
                    

                }
                else if (direzione == Direzione.Destra)
                {
                    
                    nuovaTesta.X = testaAttuale.X + 1;

                }
                else
                {
                    
                    nuovaTesta.X = testaAttuale.X - 1;
                }
                    if (nuovaTesta.X < 0 || nuovaTesta.X >= colonne || nuovaTesta.Y < 0 || nuovaTesta.Y >= righe)
                    {
                        fine = true;
                    
                    
                    }
                bool mangia = (nuovaTesta.X == cibo.X && nuovaTesta.Y == cibo.Y);
                if (fine == false)
                {

                
                    int controlli;
                    if (mangia == true)
                    {
                        controlli = snake.Count;
                    }
                    else
                    {
                        controlli = snake.Count - 1;
                    }
                    for (int i = 0; i < controlli; i++)
                    {
                        if (snake[i] == nuovaTesta)
                        {
                            fine = true;
                            break;
                        }
                    }
                }
                if (fine == false)
                {
                    snake.Insert(0, nuovaTesta);//simile ad add, ma si può decidere dove inserire l'elemento  
                    dataGridView1.Rows[nuovaTesta.Y].Cells[nuovaTesta.X].Style.BackColor = Color.Green;

                    if (mangia == true)
                    {
                        punteggio++;
                        labelPunteggio.Text = $"Punteggio: {punteggio}";
                        GeneraCibo();
                    }
                    else if (snake.Count > 0)
                    {
                        Point vecchiaCoda = snake[snake.Count - 1];
                        snake.RemoveAt(snake.Count - 1);
                        dataGridView1.Rows[vecchiaCoda.Y].Cells[vecchiaCoda.X].Style.BackColor = Color.Black;
                    }
                }
            

                dataGridView1.Refresh();//ridisegna immediatamente il form1, per aggiornare i nuovi colori del serpente prima del game over
                if (fine == true)
                {
                    timer1.Stop();
                    MessageBox.Show($"Game Over \n Punteggio: {punteggio}");
                    Start();
                    return;
                } 
            }
        }
    }