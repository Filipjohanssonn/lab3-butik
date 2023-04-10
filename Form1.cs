using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace Laboration_4
{
    public partial class Form1 : Form
    {
        //Sstores the data for Books, games and movies.
        private List<Book> books;
        private List<Game> games;
        private List<Movie> movies;

        private CsvConfiguration csvConfig;
        private CsvWriter csvWriter;
        private CsvReader csvReaderBook, csvReaderGame, csvReaderMovie;

        public Form1()
        {
            InitializeComponent();

            csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ",",
                HasHeaderRecord = true,
            };

            csvReaderBook = new CsvReader(new StreamReader("bookData.csv"), csvConfig);
            csvReaderGame = new CsvReader(new StreamReader("gameData.csv"), csvConfig);
            csvReaderMovie = new CsvReader(new StreamReader("movieData.csv"), csvConfig);
            csvReaderBook.Context.RegisterClassMap<BookMap>();
            csvReaderGame.Context.RegisterClassMap<GameMap>();
            csvReaderMovie.Context.RegisterClassMap<MovieMap>();

            // Initialize the lists here
            books = new List<Book>();
            games = new List<Game>();
            movies = new List<Movie>();

            LoadData();
        }
        
        private void LoadData()
        {
            // clear the existing data
            dataGridView1.Rows.Clear();
            dataGridView2.Rows.Clear();
            dataGridView3.Rows.Clear();
            books.Clear();
            games.Clear();
            movies.Clear();

           

            StreamReader reader = null;
            try
            {
                reader = new StreamReader("bookData.csv");
                using (var csv = new CsvReader(reader, csvConfig))
                {
                    csv.Context.RegisterClassMap<BookMap>();
                    books = csv.GetRecords<Book>().ToList();
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            try
            {
                reader = new StreamReader("gameData.csv");
                using (var csv = new CsvReader(reader, csvConfig))
                {
                    csv.Context.RegisterClassMap<GameMap>();
                    games = csv.GetRecords<Game>().ToList();
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }


            try
            {
                reader = new StreamReader("movieData.csv");
                using (var csv = new CsvReader(reader, csvConfig))
                {
                    csv.Context.RegisterClassMap<MovieMap>();
                    movies = csv.GetRecords<Movie>().ToList();
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            // read the data from the CSV file for books
            /* using (var reader = new StreamReader("bookData.csv"))
             using (var csv = new CsvReader(reader, csvConfig))
             {
                 csv.Context.RegisterClassMap<BookMap>();
                 books = csv.GetRecords<Book>().ToList();
             }



             // read the data from the CSV file for games
             using (var reader = new StreamReader("gameData.csv"))
             using (var csv = new CsvReader(reader, csvConfig))
             {
                 csv.Context.RegisterClassMap<GameMap>();
                 games = csv.GetRecords<Game>().ToList();
             }

             // read the data from the CSV file for movies
             using (var reader = new StreamReader("movieData.csv"))
             using (var csv = new CsvReader(reader, csvConfig))
             {
                 csv.Context.RegisterClassMap<MovieMap>();
                 movies = csv.GetRecords<Movie>().ToList();
             } */

            // populate the dataGridView1 with the books data
            foreach (var book in books)
            {
                var row = new string[]
                {
                    book.Namn,
                    book.Pris.ToString(),
                    book.Forfattare,
                    book.Genre,
                    book.Format,
                    book.Sprak
                };
                dataGridView1.Rows.Add(row);
            }

            // populate the dataGridView2 with the games data
            foreach (var game in games)
            {
                var row = new string[]
                {
                    game.Namn,
                    game.Pris.ToString(),
                    game.Plattform
                };
                dataGridView2.Rows.Add(row);
            }

            // populate the dataGridView3 with the movies data
            foreach (var movie in movies)
            {
                var row = new string[]
                {
                    movie.Namn,
                    movie.Pris.ToString(),
                    movie.Format,
                    movie.Speltid
                };
                dataGridView3.Rows.Add(row);
            }  
        }

        private void SaveData()
        {


            System.Threading.Thread.Sleep(1000);

            using (StreamWriter sw = new StreamWriter("bookData.csv", false, Encoding.UTF8))
            {
                using (var csv = new CsvWriter(sw, csvConfig))
                {
                    csv.Context.RegisterClassMap<BookMap>();
                    csv.WriteRecords(books);
                }

                sw.Close();
            }

            using (StreamWriter sw = new StreamWriter("gameData.csv", false, Encoding.UTF8))
            {
                using (var csv = new CsvWriter(sw, csvConfig))
                {
                    csv.Context.RegisterClassMap<GameMap>();
                    csv.WriteRecords(games);
                }
                sw.Close();
            }

            using (StreamWriter sw = new StreamWriter("movieData.csv", false, Encoding.UTF8))
            {
                using (var csv = new CsvWriter(sw, csvConfig))
                {
                    csv.Context.RegisterClassMap<MovieMap>();
                    csv.WriteRecords(movies);
                }
                sw.Close();
            }

            
        }



        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            SaveData(); // save the data when closing the form
            base.OnFormClosing(e);
        }

        // define a class to represent the "Filmer" data
        public class Movie
        {
            public string Namn { get; set; }
            public int Pris { get; set; }
            public string Format { get; set; }
            public string Speltid { get; set; }
        }

        public class Book
        {
            public string Namn { get; set; }
            public int Pris { get; set; }
            public string Forfattare { get; set; }
            public string Genre { get; set; }
            public string Format { get; set; }
            public string Sprak { get; set; }
        }

        public class Game
        {
            public string Namn { get; set; }
            public int Pris { get; set; }
            public string Plattform { get; set; }
        }


        public class MovieMap : ClassMap<Movie>
        {
            public MovieMap()
            {
                Map(m => m.Namn).Name("Namn");
                Map(m => m.Pris).Name("Pris");
                Map(m => m.Format).Name("Format");
                Map(m => m.Speltid).Name("Speltid");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveData();
        }

        public class BookMap : ClassMap<Book>
        {
            public BookMap()
            {
                Map(m => m.Namn).Name("Namn");
                Map(m => m.Pris).Name("Pris");
                Map(m => m.Forfattare).Name("Forfattare");
                Map(m => m.Genre).Name("Genre");
                Map(m => m.Format).Name("Format");
                Map(m => m.Sprak).Name("Sprak");
            }
        }

        public class GameMap : ClassMap<Game>
        {
            public GameMap()
            {
                Map(m => m.Namn).Name("Namn");
                Map(m => m.Pris).Name("Pris");
                Map(m => m.Plattform).Name("Plattform");
            }
        }
    }
}
