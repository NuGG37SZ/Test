using System;
using System.Windows.Forms;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Drawing;

namespace Airoport
{
    public partial class Main : Form
    {
        private SQLiteDataAdapter adapter;

        private DataTable dataTable;

        public Main()
        {
            InitializeComponent();
            AircraftButton.CheckedChanged += new EventHandler(CheckActiveRadioButton);
            RoutesButton.CheckedChanged += new EventHandler(CheckActiveRadioButton);
            FlightsButton.CheckedChanged += new EventHandler(CheckActiveRadioButton);
            FirstRequest.CheckedChanged += new EventHandler(CheckActiveRadioButton);
            SecondRequest.CheckedChanged += new EventHandler(CheckActiveRadioButton);
            ThirdRequest.CheckedChanged += new EventHandler(CheckActiveRadioButton);
            FourthRequest.CheckedChanged += new EventHandler(CheckActiveRadioButton);
        }

        private void LoadMain(object sender, EventArgs e)
        {
            ConnectionDataBaseClass.Connect();
            AircraftButton.Checked = true;
        }

        private void LeaveMain(object sender, EventArgs e)
        {
            ConnectionDataBaseClass.Disconnect();
        }

        private void CheckActiveRadioButton(object sender, EventArgs e)
        {
            if (AircraftButton.Checked)
                FindAllPlanes();
            
            if (RoutesButton.Checked)
                FindAllRoutes();
            
            if (FlightsButton.Checked)
                FindAllFlights();

            if (FirstRequest.Checked)
                FirstRequestSql();

            if (SecondRequest.Checked)
                SecondRequestSql();

            if (ThirdRequest.Checked)
                ThirdRequestSql();

            if (FourthRequest.Checked)
                FourthRequestSql();
        }

        private void RequestSql(string sql)
        {
            SQLiteCommand command = new SQLiteCommand(sql, ConnectionDataBaseClass.Connection);
            adapter = new SQLiteDataAdapter(command);
            dataTable = new DataTable();
            adapter.Fill(dataTable);
            SQLiteCommandBuilder commandBuilder = new SQLiteCommandBuilder(adapter);

            bindingSource1.DataSource = dataTable;
            bindingNavigator1.BindingSource = bindingSource1;
            dataGridView1.DataSource = bindingSource1;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.RowHeadersVisible = false;
        }

        private void FindAllPlanes()
        {
            RequestSql(
                "SELECT id AS 'Номер', type AS 'Самолет', seats AS 'Число мест', speed AS 'Скорость' " +
                "FROM Planes"
            );
        }

        private void FindAllRoutes()
        {
            RequestSql(
                "SELECT id AS 'Номер', distance AS 'Расстояние', " +
                "departure_point AS 'Пункт вылета', arrival_point AS 'Пункт назначения' " +
                "FROM Routes"
            );
        }

        private void FindAllFlights()
        {
            RequestSql(
                "SELECT Flights.id AS 'Номер', Flights.route_id  AS 'Пункт назначения', " +
                "Flights.planes_id AS 'Самолет', departure_time AS 'Время вылета', " +
                "arrival_time AS 'Время прилета', tickets_sold AS 'Количество проданных билетов' " +
                "FROM Flights "
            );
        }

        private void FirstRequestSql()
        {
            RequestSql(
                "SELECT AVG(STRFTIME('%s', arrival_time) - STRFTIME('%s', departure_time)) / 60 AS 'Время полета в минутах' " +
                "FROM Flights f " +
                "JOIN Routes r ON f.route_id = r.id " +
                "JOIN Planes p ON f.planes_id = p.id " +
                "WHERE p.Type = 'ТУ-154' AND r.departure_point = 'Чугуев' AND r.arrival_point = 'Мерефа';"
            );
        }

        private void SecondRequestSql()
        {
            RequestSql(
                 "SELECT p.type " +
                  "FROM Flights f " +
                  "JOIN Routes r ON f.route_id = r.id " +
                  "JOIN Planes p ON f.planes_id = p.id " +
                  "WHERE r.departure_point = 'Чугуев' AND r.arrival_point = 'Мерефа' " +
                  "GROUP BY p.type " +
                  "ORDER BY COUNT(f.id) DESC " +
                  "LIMIT 1;"
            );
        }

        private void ThirdRequestSql()
        {
            RequestSql(
                  "SELECT r.* " +
                  "FROM Flights f " +
                  "JOIN Routes r ON f.route_id = r.id " + 
                  "JOIN Planes p ON f.planes_id = p.id " +
                  "WHERE(f.tickets_sold / CAST(p.seats AS FLOAT)) < 0.7 " +
                  "GROUP BY r.id " +
                  "ORDER BY COUNT(f.id) DESC;"
            );
        }

        private void FourthRequestSql()
        {
            RequestSql(
                  "SELECT p.seats - f.tickets_sold AS FreeSeats " +
                  "FROM Flights f " +
                  "JOIN Planes p ON f.planes_id = p.id " +
                  "WHERE f.id = 1 AND DATE(f.departure_time) = '2000-12-31';"
            );
        }

        private void ChangeBindingSource(object sender, ListChangedEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.ItemAdded || e.ListChangedType == ListChangedType.ItemDeleted ||
                e.ListChangedType == ListChangedType.ItemChanged) 
            {
                adapter.Update(dataTable);
            }
        }

    }
}
