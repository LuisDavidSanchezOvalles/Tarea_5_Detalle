using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using RegistroDetalle.BLL;
using RegistroDetalle.Entidades;

namespace RegistroDetalle.UI.Registro
{
    /// <summary>
    /// Interaction logic for RPersonas.xaml
    /// </summary>
    public partial class RPersonas : Window
    {
        Persona persona = new Persona();
        public RPersonas()
        {
            InitializeComponent();
            //se le pone o asigna la persona al objeto DataContext para hacer el binding y se pone al inicilizar
            this.DataContext = persona;
            PersonaIdTextBox.Text = "0";
        }

        private void Limpiar()
        {
            PersonaIdTextBox.Text = "0";
            NombresTextBox.Text = string.Empty;
            DireccionTextBox.Text = string.Empty;
            CedulaTextBox.Text = string.Empty;
            FechaNacimientoDatePicker.SelectedDate = DateTime.Now;
            TelefonosDataGrid.ItemsSource = new List<TelefonosDetalle>();
        }

        private void NuevoButton_Click(object sender, RoutedEventArgs e)
        {
            Limpiar();
        }

        private bool Validar()
        {
            bool paso = true;

            if (string.IsNullOrWhiteSpace(PersonaIdTextBox.Text))
                paso = false;
            else
            {
                try
                {
                    int i = Convert.ToInt32(PersonaIdTextBox.Text);
                }
                catch (FormatException)
                {
                    paso = false;
                }
            }

            if (string.IsNullOrWhiteSpace(NombresTextBox.Text))
                paso = false;
            else
            {
                foreach (var caracter in NombresTextBox.Text)
                {
                    if (!char.IsLetter(caracter) && !char.IsWhiteSpace(caracter))
                        paso = false;
                }
            }

            if (string.IsNullOrWhiteSpace(DireccionTextBox.Text))
                paso = false;

            if (string.IsNullOrWhiteSpace(CedulaTextBox.Text))
                paso = false;
            else
            {
                foreach(var caracter in CedulaTextBox.Text)
                {
                    if (!char.IsDigit(caracter))
                        paso = false;
                }
            }

            if (TelefonosDataGrid.Columns.Count == 0)
            {
                MessageBox.Show("Todos los Campos debe LLenarlo y Tiene que Agregarlo...");
                TelefonosDataGrid.Focus();
                paso = false;
            }

            if (paso == false)
                MessageBox.Show("Datos invalidos");

            return paso;
        }

        private bool ExisteEnLaBaseDeDatos()
        {
            Persona PersonaAnterior = PersonasBLL.Buscar(persona.PersonaId);

            return PersonaAnterior != null;
        }

        private void GuardarButton_Click(object sender, RoutedEventArgs e)
        {
            bool paso = false;

            if (!Validar())
                return;

            if (persona.PersonaId == 0)
                paso = PersonasBLL.Guardar(persona);
            else
            {
                if(ExisteEnLaBaseDeDatos())
                {
                    paso = PersonasBLL.Modificar(persona);
                }
                else
                {
                    MessageBox.Show("No se Puede Modificar una persona que no existe");
                    return;
                }
            }

            if(paso)
            {
                Limpiar();
                MessageBox.Show("Guardado");
            }
            else
            {
                MessageBox.Show("La Persona No se Pudo Guardar");
            }
        }

        private void EliminarButton_Click(object sender, RoutedEventArgs e)
        {
            if (PersonasBLL.Eliminar(persona.PersonaId))
            {
                MessageBox.Show("Eliminado");
                Limpiar();
            }
            else
                MessageBox.Show("No se pudo eliminar una persona que no existe");                        
        }

        private void Recargar()
        {
            this.DataContext = null;
            this.DataContext = persona;
        }

        private void BuscarButton_Click(object sender, RoutedEventArgs e)
        {
            Persona PersonaAnterior = PersonasBLL.Buscar(persona.PersonaId);

            if (PersonaAnterior != null)
            {
                persona = PersonaAnterior;
                Recargar();
            }
            else
            {
                Limpiar();
                MessageBox.Show("Persona no encontrada");
            }
        }

        private void AgregarButton_Click(object sender, RoutedEventArgs e)
        {
            persona.Telefonos.Add(new TelefonosDetalle(persona.PersonaId ,TelefonoTextBox.Text, TipoTextBox.Text));

            Recargar();

            TelefonoTextBox.Clear();
            TipoTextBox.Clear();
            TelefonoTextBox.Focus();
        }

        private void RemoverButton_Click(object sender, RoutedEventArgs e)
        {
            if (TelefonosDataGrid.Items.Count > 1 && TelefonosDataGrid.SelectedIndex < TelefonosDataGrid.Items.Count - 1)
            {
                persona.Telefonos.RemoveAt(TelefonosDataGrid.SelectedIndex);
                Recargar();
            }
        }
    }
}
