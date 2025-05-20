using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace VisualD_D
{
    public partial class MainWindow : Window
    {
        const int MAX_SLOTS = 3;
        string[] slots = new string[MAX_SLOTS];
        string[] slotPaths = new string[MAX_SLOTS];
        Adventure actualAdventure = null;

        int selectedSlot = -1;
        bool esNuevaPartida = false;

        int puntosRestantes = 15;
        int fuerza = 1, constitucion = 1, vida = 1, inteligencia = 1, velocidad = 1;

        private Inventory gameInventory = new Inventory(); 
        private Bazar bazaar = new Bazar();
        private List<ItemObject> contrabandistaOffer = new List<ItemObject>();
        private Contrabandista smuggler = new Contrabandista();
        private EscuelaMagia school = new EscuelaMagia();

        // Arrays para acceder a controles sin FindName
        private Button[] botonesHeroes;
        private Rectangle[] barrasVida;
        private Rectangle[] imagenesHeroes;

        private Button[] botonesEquipaciones;
        private Rectangle[,] fotosEquipaciones;
        private Rectangle[,] vidasEquipaciones;



        public MainWindow()
        {
            InitializeComponent();

            // Inicializar arrays de héroes
            botonesHeroes = new Button[] { NombreHeroe1, NombreHeroe2, NombreHeroe3, NombreHeroe4, NombreHeroe5, NombreHeroe6 };
            barrasVida = new Rectangle[] { VidaHeroe1, VidaHeroe2, VidaHeroe3, VidaHeroe4, VidaHeroe5, VidaHeroe6 };
            imagenesHeroes = new Rectangle[] { FutureHeroeImage1, FutureHeroeImage2, FutureHeroeImage3, FutureHeroeImage4, FutureHeroeImage5, FutureHeroeImage6 };

            // Inicializar arrays de equipaciones
            botonesEquipaciones = new Button[] { NombreEquipacion1, NombreEquipacion2, NombreEquipacion3, NombreEquipacion4, NombreEquipacion5 };
            fotosEquipaciones = new Rectangle[5, 5];
            vidasEquipaciones = new Rectangle[5, 5];

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    fotosEquipaciones[i, j] = (Rectangle)FindName($"Equipations{i + 1}HeroPhoto{j + 1}");
                    vidasEquipaciones[i, j] = (Rectangle)FindName($"Equipations{i + 1}HeroLife{j + 1}");
                }
            }


            IniSlots();
            InitializeInventory(gameInventory);
            LoadBazarItems();
        }

        //Prev Methods
        private void IniSlots()
        {
            string savesFolder = System.IO.Path.Combine(Environment.CurrentDirectory, "Saves");
            Directory.CreateDirectory(savesFolder);

            for (int counterSlots = 0; counterSlots < MAX_SLOTS; counterSlots++)
            {
                slotPaths[counterSlots] = System.IO.Path.Combine(savesFolder, "slot" + (counterSlots + 1) + ".dat");
                slots[counterSlots] = File.Exists(slotPaths[counterSlots]) ? GetAdventureNameByFile(slotPaths[counterSlots]) : "Vacío";
            }

            SlotButton1.Content = "Slot 1 - " + slots[0];
            SlotButton2.Content = "Slot 2 - " + slots[1];
            SlotButton3.Content = "Slot 3 - " + slots[2];
        }
        static string GetAdventureNameByFile(string ruta)
        {
            try
            {
                using (FileStream fs = new FileStream(ruta, FileMode.Open))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    Adventure aventura = (Adventure)bf.Deserialize(fs);
                    return aventura.GetAdventureName();
                }
            }
            catch
            {
                return "CORRUPTO";
            }
        }
        public static void GuardarAventura(Adventure aventura, string ruta)
        {
            FileStream stream = null;
            try
            {
                stream = new FileStream(ruta, FileMode.Create);
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, aventura);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar la aventura: " + ex.Message);
            }
            if (stream != null)
            {
                stream.Close();
            }
        }
        public static Adventure CargarAventura(string ruta)
        {
            FileStream stream = null;
            Adventure aventuraCargada = null;
            try
            {
                stream = new FileStream(ruta, FileMode.Open);
                BinaryFormatter formatter = new BinaryFormatter();
                aventuraCargada = (Adventure)formatter.Deserialize(stream);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar la aventura: " + ex.Message);
            }
            if (stream != null)
            {
                stream.Close();
            }

            return aventuraCargada;
        }

        //First menu
        private void NewAdventureClick(object sender, RoutedEventArgs e)
        {
            esNuevaPartida = true;
            MainMenu.Visibility = Visibility.Collapsed;
            SlotMenu.Visibility = Visibility.Visible;
        }
        private void ContinueAdventureClick(object sender, RoutedEventArgs e)
        {
            esNuevaPartida = false;
            MainMenu.Visibility = Visibility.Collapsed;
            SlotMenu.Visibility = Visibility.Visible;
        }

        private void ExitClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        //Second menu
        private void SlotClick(object sender, RoutedEventArgs e)
        {
            Button slotButton = sender as Button;

            if (slotButton == SlotButton1) selectedSlot = 0;
            else if (slotButton == SlotButton2) selectedSlot = 1;
            else if (slotButton == SlotButton3) selectedSlot = 2;

            string path = slotPaths[selectedSlot];
            string nombreExistente = slots[selectedSlot];

            if (esNuevaPartida)
            {
                if (File.Exists(path))
                {
                    DialogText.Text = "¿Deseas sobreescribir la partida " + nombreExistente + "?";
                    MostrarDialogo();
                }
                else
                {
                    MostrarCrearAventuraPanel();
                }
            }
            else
            {
                if (File.Exists(path))
                {
                    actualAdventure = CargarAventura(path);
                    actualAdventure.StartSession();
                    MessageBox.Show("Partida cargada correctamente.");
                    MostrarAdventurePanel();
                }
                else
                {
                    DialogText.Text = "No hay partida en este slot. ¿Deseas crear una nueva?";
                    MostrarDialogo();
                }
            }
        }

        //Protagonist menu
        private void MostrarCrearAventuraPanel()
        {
            MainMenu.Visibility = Visibility.Collapsed;
            SlotMenu.Visibility = Visibility.Collapsed;
            CrearAventuraPanel.Visibility = Visibility.Visible;
        }

        //Confirmation menu
        private void YesClick(object sender, RoutedEventArgs e)
        {
            ConfirmationDialog.Visibility = Visibility.Collapsed;

            if (esNuevaPartida || !File.Exists(slotPaths[selectedSlot]))
            {
                MostrarCrearAventuraPanel(); // <-- antes llamabas directamente a CrearNuevaPartida()
            }

        }
        private void NoClick(object sender, RoutedEventArgs e)
        {
            VolverAMenuSlots();
        }

        //City Menu
        private void MostrarAdventurePanel()
        {
            CiudadStack.Visibility = Visibility.Visible;
            AdventureLabel.Visibility = Visibility.Collapsed;
            CrearAventuraPanel.Visibility = Visibility.Collapsed;
            MainMenu.Visibility = Visibility.Collapsed;
            SlotMenu.Visibility = Visibility.Collapsed;

            UpdateGold();
        }
        private void SaveExitClick(object sender, RoutedEventArgs e)
        {
            if (actualAdventure != null && selectedSlot >= 0)
            {
                actualAdventure.EndSession();
                GuardarAventura(actualAdventure, slotPaths[selectedSlot]);
                MessageBox.Show("Partida guardada.");
            }

            // Volver al menú principal
            CiudadStack.Visibility = Visibility.Collapsed;
            MainMenu.Visibility = Visibility.Visible;
        }
        private void SaveClick(object sender, RoutedEventArgs e)
        {
            if (actualAdventure != null && selectedSlot >= 0)
            {
                actualAdventure.EndSession();
                GuardarAventura(actualAdventure, slotPaths[selectedSlot]);
                MessageBox.Show("Partida guardada.");
            }
        }
        private void DungeonClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("¡Te adentras en la mazmorra! (aquí puedes lanzar una escena o minijuego)");
            // Aquí podrías llamar a una función de aventura o evento
        }
        private void Bazaar(object sender, RoutedEventArgs e)
        {
            // Solo actualizamos el bazar si no tiene objetos disponibles ya, para persistencia dentro de la sesión
            if (bazaar.GetObjetosDisponibles().Count == 0)
            {
                List<ItemObject> comunes = gameInventory.GetItems()
                    .OfType<ItemObject>()
                    .Where(obj => obj.GetTier() == 1)
                    .ToList();
                if (comunes.Count == 0)
                {
                    MessageBox.Show("No hay objetos disponibles en el bazar.");
                    return;
                }
                bazaar.Actualizar(comunes);
            }
            var disponibles = bazaar.GetObjetosDisponibles();
            // Mostrar botones y precios de forma segura para evitar errores de índice
            for (int i = 0; i < 5; i++)
            {
                Button btn = (Button)FindName($"NombreItem{i + 1}");
                Label txt = (Label)FindName($"GoldItem{i + 1}");
                if (i < disponibles.Count)
                {
                    btn.Content = disponibles[i].GetName();
                    btn.Visibility = Visibility.Visible;
                    txt.Content = disponibles[i].GetBuyValue().ToString();
                    txt.Visibility = Visibility.Visible;
                }
                else
                {
                    btn.Visibility = Visibility.Hidden;
                    txt.Visibility = Visibility.Hidden;
                }
            }
            UpdateGold(); // Actualiza tanto GoldText como GoldTextBazar
            BazarStack.Visibility = Visibility.Visible;
            CiudadStack.Visibility = Visibility.Hidden;
        }
        private void SmugglerClick(object sender, RoutedEventArgs e)
        {
            if (actualAdventure.GetGold() < 10)
            {
                MessageBox.Show("No tienes suficiente oro para entrar al Contrabandista (10).");
                return;
            }

            actualAdventure.SpendGold(10);
            UpdateGold();

            List<ItemObject> unicos = gameInventory.GetItems()
                .OfType<ItemObject>()
                .Where(obj => obj.GetTier() == 2)
                .ToList();

            contrabandistaOffer = smuggler.CreateOffer(actualAdventure, unicos);

            if (contrabandistaOffer.Count == 0)
            {
                MessageBox.Show("No quedan objetos únicos disponibles para ofrecerte.");
                return;
            }

            ShowContrabandistaOffer();

            // Mostrar panel contrabandista y ocultar ciudad
            ContrabandistaStack.Visibility = Visibility.Visible;
            CiudadStack.Visibility = Visibility.Collapsed;
        }
        private void ShowContrabandistaOffer()
        {
            for (int i = 0; i < 3; i++)
            {
                Button btn = (Button)FindName($"ContrabandistaItem{i + 1}");
                Label txt = (Label)FindName($"ContrabandistaGoldItem{i + 1}");

                if (i < contrabandistaOffer.Count)
                {
                    btn.Content = contrabandistaOffer[i].GetName();
                    btn.Visibility = Visibility.Visible;
                    txt.Content = contrabandistaOffer[i].GetBuyValue().ToString();
                    txt.Visibility = Visibility.Visible;
                    btn.IsEnabled = true;
                }
                else
                {
                    btn.Visibility = Visibility.Hidden;
                    txt.Visibility = Visibility.Hidden;
                }
            }
        }
        private void BuyContrabandistaItem(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null) return;

            // Determine index from button name like "ContrabandistaItem1"
            string name = btn.Name;
            int index = -1;
            if (name.StartsWith("ContrabandistaItem") && int.TryParse(name.Substring("ContrabandistaItem".Length), out int parsedIndex))
                index = parsedIndex - 1;
            else
                return; // unknown button

            if (index < 0 || index >= contrabandistaOffer.Count)
            {
                MessageBox.Show("Índice fuera de rango.");
                return;
            }

            bool success = smuggler.BuyItem(actualAdventure, index);
            if (success)
            {
                MessageBox.Show($"Has comprado {contrabandistaOffer[index].GetName()}.");
                UpdateGold();
                contrabandistaOffer.RemoveAt(index);
                ShowContrabandistaOffer(); // Refresh UI offer list
            }
            else
            {
                MessageBox.Show("No tienes suficiente oro para comprar este objeto.");
            }
        }
        private void MagicSchoolClick(object sender, RoutedEventArgs e)
        {
            if (!actualAdventure.HasPaidMagicSchoolAccess())
            {
                if (actualAdventure.GetGold() < 15)
                {
                    MessageBox.Show("No tienes suficiente oro para acceder a la Escuela de Magia (15).");
                    return;
                }

                actualAdventure.SpendGold(15);
                actualAdventure.SetMagicSchoolAccessPaid(); // Marcar como pagado
                UpdateGold();
                MessageBox.Show("Has pagado por acceder a la Escuela de Magia.");
            }

            school.Actualizar(gameInventory.GetItems().OfType<Spell>().ToList()); // Refrescar lista de hechizos
            ShowMagicSchoolOffer(); // Mostrar la interfaz
        }
        private void TownSquareClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Te encuentras con los aldeanos y escuchas rumores del bosque...");
        }
        private void HealerClick(object sender, RoutedEventArgs e)
        {
            CiudadStack.Visibility = Visibility.Collapsed;
            TavernaStack.Visibility = Visibility.Visible;
        }

        //Bazar menu
        private void BuyItem1(object sender, RoutedEventArgs e) => ComprarDesdeBazar(0);
        private void BuyItem2(object sender, RoutedEventArgs e) => ComprarDesdeBazar(1);
        private void BuyItem3(object sender, RoutedEventArgs e) => ComprarDesdeBazar(2);
        private void BuyItem4(object sender, RoutedEventArgs e) => ComprarDesdeBazar(3);
        private void BuyItem5(object sender, RoutedEventArgs e) => ComprarDesdeBazar(4);
        private void GoToCity(object sender, RoutedEventArgs e)
        {
            BazarStack.Visibility = Visibility.Hidden;
            ContrabandistaStack.Visibility = Visibility.Hidden;
            MagicSchoolStack.Visibility = Visibility.Hidden;
            TavernaStack.Visibility = Visibility.Hidden;
            CiudadStack.Visibility = Visibility.Visible;
        }

        //Taberna
        private void HealAllHeroesClick(object sender, RoutedEventArgs e)
        {
            if (actualAdventure == null)
            {
                MessageBox.Show("No hay aventura activa.");
                return;
            }

            var heroes = actualAdventure.GetHeroes();
            ProcesarCuracion(heroes);

            RefreshManualHeroHealing();
            RefreshFormacionHealing();
        }
        private void CurarHeroe(object sender, RoutedEventArgs e)
        {
            if (actualAdventure == null)
            {
                MessageBox.Show("No hay aventura activa.");
                return;
            }

            var hero = (Hero)((Button)sender).Tag;
            ProcesarCuracion(new List<Hero> { hero });
            RefreshManualHeroHealing();
        }
        private void CurarEquipacion(object sender, RoutedEventArgs e)
        {
            if (actualAdventure == null)
            {
                MessageBox.Show("No hay aventura activa.");
                return;
            }

            var formacion = (Equipacion)((Button)sender).Tag;
            ProcesarCuracion(formacion.GetHeroes());
            RefreshFormacionHealing();
        }
        private void ProcesarCuracion(List<Hero> heroes)
        {
            if (actualAdventure == null)
            {
                MessageBox.Show("No hay aventura activa.");
                return;
            }

            int oroAntes = actualAdventure.GetGold();
            int oroRestante = Healer(oroAntes, heroes);

            if (oroRestante == -1)
            {
                MessageBox.Show("No tienes suficiente oro para curar a tus héroes y todos estaban muertos.");
            }
            else if (oroRestante == oroAntes)
            {
                MessageBox.Show("Todos tus héroes ya están con la vida al máximo.");
            }
            else
            {
                int oroGastado = oroAntes - oroRestante;
                actualAdventure.SpendGold(oroGastado);
                UpdateGold();
                MessageBox.Show($"Curación completada. Te quedan: {oroRestante} de oro.");
            }
        }
        private int Healer(int totalGold, List<Hero> heroes)
        {
            bool anyRevived = false;
            bool anyHealed = false;
            int totalHealingCost = 0;
            int deadCount = heroes.Count(h => h.GetActualLife() <= 0);
            bool allWereDead = deadCount == heroes.Count && deadCount > 0;

            foreach (Hero hero in heroes)
            {
                int maxHealth = hero.GetHP();
                int currentHealth = hero.GetActualLife();

                if (maxHealth <= 0) continue; // Evitar división por cero

                if (currentHealth <= 0)
                {
                    totalHealingCost += 10;
                    anyRevived = true;
                }
                else if (currentHealth < maxHealth)
                {
                    int percentage = (currentHealth * 100) / maxHealth;
                    int healingCost = percentage >= 75 ? 2 :
                                      percentage >= 50 ? 4 :
                                      percentage >= 25 ? 6 : 8;
                    totalHealingCost += healingCost;
                    anyHealed = true;
                }
            }

            if (totalHealingCost == 0)
                return totalGold;

            if (totalGold >= totalHealingCost)
            {
                totalGold -= totalHealingCost;
                foreach (Hero hero in heroes)
                {
                    hero.SetActualHP(hero.GetHP());
                }

                if (allWereDead)
                    MessageBox.Show("Todos los héroes han sido revividos. Que no vuelva a ocurrir.");
                else if (anyRevived)
                    MessageBox.Show("¡Tus personajes fueron revividos!");
                else if (anyHealed)
                    MessageBox.Show("¡Tus personajes fueron curados con éxito!");

                return totalGold;
            }

            return allWereDead ? -1 : totalGold;
        }
        private void RefreshManualHeroHealing()
        {
            if (actualAdventure == null) return;

            var heroes = actualAdventure.GetHeroes();

            for (int i = 0; i < botonesHeroes.Length; i++)
            {
                if (i < heroes.Count)
                {
                    var hero = heroes[i];
                    botonesHeroes[i].Content = hero.GetName();
                    botonesHeroes[i].Tag = hero;
                    botonesHeroes[i].Visibility = Visibility.Visible;

                    double vida = hero.GetHP() > 0 ? (double)hero.GetActualLife() / hero.GetHP() : 0;
                    barrasVida[i].Width = 100 * vida;
                    barrasVida[i].Visibility = Visibility.Visible;
                    imagenesHeroes[i].Visibility = Visibility.Visible;
                }
                else
                {
                    botonesHeroes[i].Visibility = Visibility.Hidden;
                    barrasVida[i].Visibility = Visibility.Hidden;
                    imagenesHeroes[i].Visibility = Visibility.Hidden;
                }
            }
        }
        private void RefreshFormacionHealing()
        {
            if (actualAdventure == null) return;

            var formaciones = actualAdventure.GetFormations();

            for (int i = 0; i < botonesEquipaciones.Length; i++)
            {
                if (i < formaciones.Count)
                {
                    var formacion = formaciones[i];
                    botonesEquipaciones[i].Content = formacion.GetName();
                    botonesEquipaciones[i].Tag = formacion;
                    botonesEquipaciones[i].Visibility = Visibility.Visible;

                    var heroes = formacion.GetHeroes();

                    for (int j = 0; j < 5; j++)
                    {
                        if (j < heroes.Count)
                        {
                            var h = heroes[j];
                            fotosEquipaciones[i, j].Visibility = Visibility.Visible;
                            vidasEquipaciones[i, j].Visibility = Visibility.Visible;

                            double vida = h.GetHP() > 0 ? (double)h.GetActualLife() / h.GetHP() : 0;
                            vidasEquipaciones[i, j].Width = 22 * vida;
                        }
                        else
                        {
                            fotosEquipaciones[i, j].Visibility = Visibility.Hidden;
                            vidasEquipaciones[i, j].Visibility = Visibility.Hidden;
                        }
                    }
                }
                else
                {
                    botonesEquipaciones[i].Visibility = Visibility.Hidden;

                    for (int j = 0; j < 5; j++)
                    {
                        fotosEquipaciones[i, j].Visibility = Visibility.Hidden;
                        vidasEquipaciones[i, j].Visibility = Visibility.Hidden;
                    }
                }
            }
        }
        private void SelectHeroesClick(object sender, RoutedEventArgs e)
        {
            TavernaStack.Visibility = Visibility.Hidden;
            ManualyHeroesStack.Visibility = Visibility.Visible;
            RefreshManualHeroHealing();
        }
        private void SelectEquipmentClick(object sender, RoutedEventArgs e)
        {
            TavernaStack.Visibility = Visibility.Hidden;
            EquipacionStack.Visibility = Visibility.Visible;
            RefreshFormacionHealing();
        }



        //Aux
        private void VolverAMenuSlots()
        {
            ConfirmationDialog.Visibility = Visibility.Collapsed;
            SlotMenu.Visibility = Visibility.Visible;
        }
        private void MostrarDialogo()
        {
            SlotMenu.Visibility = Visibility.Collapsed;
            ConfirmationDialog.Visibility = Visibility.Visible;
        }
        private void ConfirmarCrearAventura(object sender, RoutedEventArgs e)
        {
            string nombreAventura = NombreAventuraBox.Text.ToString();
            string nombreHeroe = NombreHeroeBox.Text.ToString();
            string titulo = TituloHeroeBox.Text.ToString();
            string raza = (RazaCombo.SelectedItem as ComboBoxItem)?.Content.ToString();
            string clase = (ClaseCombo.SelectedItem as ComboBoxItem)?.Content.ToString();
            string rol = LoreBox.Text.ToString();

            if (string.IsNullOrEmpty(nombreAventura) || string.IsNullOrEmpty(nombreHeroe) || raza == null || clase == null)
            {
                MessageBox.Show("Por favor, completa todos los campos requeridos.");
                return;
            }

            foreach (string nombre in slots)
            {
                if (nombre == nombreAventura)
                {
                    MessageBox.Show("Ya existe una aventura con ese nombre. Por favor elige otro nombre.");
                    return;
                }
            }

            actualAdventure = new Adventure(nombreAventura, 0);
            actualAdventure.StartSession();

            Hero protagonista = new Hero(nombreHeroe, titulo, raza, clase, rol, fuerza, constitucion, vida, inteligencia, velocidad, true);
            actualAdventure.SetProtagonist(protagonista);

            // Aquí podrías añadir NPCs si quieres, como antes
            actualAdventure.AddHero(new Hero("Thalendir", "El Escudo", "Humano", "Guerrero", "Capitán", 5, 3, 5, 2, 4));
            actualAdventure.AddHero(new Hero("Elandra", "La Dama", "Elfa", "Maga", "Guardiana", 4, 2, 4, 5, 6));
            actualAdventure.AddHero(new Hero("Gorak", "Puño de Hierro", "Orco", "Sanador", "Chamán", 3, 5, 6, 2, 3));

            actualAdventure.EndSession();
            GuardarAventura(actualAdventure, slotPaths[selectedSlot]);
            slots[selectedSlot] = nombreAventura;

            IniSlots();

            CrearAventuraPanel.Visibility = Visibility.Collapsed;
            AdventureLabel.Visibility = Visibility.Visible;
            AdventureLabel.Content = $"Aventura: {nombreAventura}";
            MostrarAdventurePanel();
        }
        private void IncrementarStat(object sender, RoutedEventArgs e)
        {
            if (puntosRestantes <= 0) return;

            Button boton = sender as Button;
            if (boton == BtnFuerza) { fuerza++; boton.Content = $"Fuerza: {fuerza}"; }
            else if (boton == BtnConstitucion) { constitucion++; boton.Content = $"Constitución: {constitucion}"; }
            else if (boton == BtnVida) { vida++; boton.Content = $"Vida: {vida}"; }
            else if (boton == BtnInteligencia) { inteligencia++; boton.Content = $"Inteligencia: {inteligencia}"; }
            else if (boton == BtnVelocidad) { velocidad++; boton.Content = $"Velocidad: {velocidad}"; }

            puntosRestantes--;
            PuntosRestantesTextBlock.Text = $"Puntos restantes: {puntosRestantes}";
        }
        private void UpdateGold()
        {
            var oro = actualAdventure?.GetGold() ?? 0;
            GoldText.Content = oro.ToString(); // Oro en la ciudad
            GoldTextBazar.Content = oro.ToString(); // Oro en el bazar
            GoldTextSmuggler.Content = oro.ToString(); // Oro en el contrabandista
            GoldTextSchool.Content = oro.ToString(); // Oro en la escuela de magia
        }
        private void ComprarDesdeBazar(int index)
        {
            var disponibles = bazaar.GetObjetosDisponibles();
            if (index < 0 || index >= disponibles.Count)
            {
                MessageBox.Show("Índice fuera de rango.");
                return;
            }
            bool comprado = bazaar.ComprarItemPorIndice(actualAdventure, index);
            if (comprado)
            {
                // Ocultar botón y textbox correspondiente
                Button btn = (Button)FindName($"NombreItem{index + 1}");
                Label txt = (Label)FindName($"GoldItem{index + 1}");
                btn.Visibility = Visibility.Hidden;
                txt.Visibility = Visibility.Hidden;
                UpdateGold();
                MessageBox.Show("¡Compra realizada con éxito!");
                // Actualizar visual del bazar para reflejar la compra (botones y precios)
                RefreshBazarDisplay();
            }
            else
            {
                MessageBox.Show("No tienes suficiente oro para comprar este objeto.");
            }
        }
        private void RefreshBazarDisplay()
        {
            var disponibles = bazaar.GetObjetosDisponibles();
            for (int i = 0; i < 5; i++)
            {
                Button btn = (Button)FindName($"NombreItem{i + 1}");
                Label txt = (Label)FindName($"GoldItem{i + 1}");
                if (i < disponibles.Count)
                {
                    btn.Content = disponibles[i].GetName();
                    btn.Visibility = Visibility.Visible;
                    txt.Content = disponibles[i].GetBuyValue().ToString();
                    txt.Visibility = Visibility.Visible;
                }
                else
                {
                    btn.Visibility = Visibility.Hidden;
                    txt.Visibility = Visibility.Hidden;
                }
            }
        }

        private void GoToTaverna(object sender, RoutedEventArgs e)
        {
            EquipacionStack.Visibility = Visibility.Collapsed;
            ManualyHeroesStack.Visibility = Visibility.Collapsed;
            TavernaStack.Visibility = Visibility.Visible;
        }
        private void NextCarrousell(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Siguiente");
        }
        private void PrevCarrousell(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Anterior");
        }

        private void LoadBazarItems()
        {
            // Si quieres persisitir bazar entre sesiones, deberás guardar y cargar
            // la lista de objetos disponibles aquí. Por ahora solo limpia para nueva sesiòn.
            if (bazaar.GetObjetosDisponibles() == null || bazaar.GetObjetosDisponibles().Count == 0)
            {
                bazaar.Actualizar(
                    gameInventory.GetItems()
                    .OfType<ItemObject>()
                    .Where(obj => obj.GetTier() == 1)
                    .ToList()
                );
            }
        }
        private void ShowMagicSchoolOffer()
        {
            var hechizos = school.GetHechizos();
            for (int i = 0; i < 4; i++) // Cambiado a 4 para incluir el cuarto hechizo
            {
                Button btn = (Button)FindName($"HechizoItem{i + 1}");
                Label lbl = (Label)FindName($"HechizoGoldItem{i + 1}");
                if (i < hechizos.Count)
                {
                    btn.Content = hechizos[i].GetName();
                    lbl.Content = hechizos[i].GetBuyValue().ToString();
                    btn.Visibility = Visibility.Visible;
                    lbl.Visibility = Visibility.Visible;
                }
                else
                {
                    btn.Visibility = Visibility.Hidden;
                    lbl.Visibility = Visibility.Hidden;
                }
            }
            MagicSchoolStack.Visibility = Visibility.Visible; // Asegúrate de que este StackPanel esté definido en tu XAML
            CiudadStack.Visibility = Visibility.Hidden; // Oculta la ciudad
        }
        private void BuyMagicSchoolSpell(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null) return;
            int index = int.Parse(btn.Name.Substring("HechizoItem".Length)) - 1; // Obtener el índice del hechizo
            school.ComprarHechizo(actualAdventure, index);
            UpdateGold();
            ShowMagicSchoolOffer(); // Actualiza la oferta de hechizos
        }
        static void InitializeInventory(Inventory inventory)
        {
            // Objetos comunes (Físicos)
            inventory.AddItem(new ItemObject("Espada de Acero", 20, 10, 1, false, 2, 0, 0, 0, 0, 1, "Arma"));
            inventory.AddItem(new ItemObject("Espada Roma", 5, 1, 1, false, 1, 0, 0, 0, 0, 1, "Arma"));
            inventory.AddItem(new ItemObject("Escudo de Madera", 15, 8, 1, false, 0, 2, 0, 0, 2, 1, "Escudo"));
            // Objetos comunes (Mágicos)
            inventory.AddItem(new ItemObject("Escudo Mágico", 50, 25, 1, true, 0, 3, 0, 0, 2, 1, "Escudo"));
            inventory.AddItem(new ItemObject("Poción de Curación", 10, 5, 0, true, 0, 0, 0, 0, 2, 1, "Pocion"));
            inventory.AddItem(new ItemObject("Poción de Constitución", 15, 5, 0, true, 0, 1, 0, 0, 0, 1, "Pocion"));
            // Objetos únicos (Físicos)
            inventory.AddItem(new ItemObject("Espada Gigante", 500, 100, 2, false, 6, 0, 0, 0, 0, 2, "Arma"));
            inventory.AddItem(new ItemObject("Daga Oscura", 700, 600, 1, false, 4, 0, 0, 0, 0, 2, "Arma"));
            // Objetos únicos (Mágicos)
            inventory.AddItem(new ItemObject("Espada Legendaria", 1000, 90, 2, true, 6, 0, 0, 0, 0, 2, "Arma"));
            inventory.AddItem(new ItemObject("Escudo de la Eternidad", 160, 80, 2, true, 0, 4, 0, 0, 0, 2, "Escudo"));
            // Hechizos
            inventory.AddItem(new Spell("Hechizo de Fuego", 30, 10, 0, 2, 3));
            inventory.AddItem(new Spell("Hechizo de Fortalecimiento", 25, 8, 1, 1, 2));
            inventory.AddItem(new Spell("Hechizo de Escudo Mental", 30, 10, 0, 2, 2));
            inventory.AddItem(new Spell("Hechizo de Curación Mayor", 40, 15, 1, 3, 4));
        }
    }
}
