using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace D_D_code
{
    public abstract class Item
    {
        // Protected attributes
        protected string itemName;
        protected int itemBuyValue;
        protected int itemSellValue;
        protected int itemHandsRequired;

        // Constructor
        public Item(string intName, int intBuyValue, int intSellValue, int intHandsRequired)
        {
            itemName = intName;
            itemBuyValue = intBuyValue;
            itemSellValue = intSellValue;
            itemHandsRequired = intHandsRequired;
        }

        // Getters
        public string GetName()
        {
            return itemName;
        }

        public int GetBuyValue()
        {
            return itemBuyValue;
        }

        public int GetSellValue()
        {
            return itemSellValue;
        }

        public int GetHandsRequired()
        {
            return itemHandsRequired;
        }

        // Modificadores
        public virtual bool IsOffensive()
        {
            return false;
        }

        public virtual bool IsEquipable()
        {
            return false; // Por defecto, no es equipable
        }

        public virtual bool HasEffects()
        {
            return false; // Por defecto, no tiene efectos
        }

        // Abstract methods
        public abstract int GetBonus();
        public abstract bool GetIsConsumable();
        public abstract void Use(Hero targetHero, Enemy targetEnemy);
        public abstract void ShowInfo();
    }
    public class Object : Item
    {
        private bool isConsumable;
        private bool isMagical;
        private int bonusStrength;
        private int bonusHP;

        public Object(string intName, int intBuyValue, int intSellValue, int intHandsRequired, bool intIsMagical, bool intIsConsumable, int intBonusStrength, int intBonusHP)
            : base(intName, intBuyValue, intSellValue, intHandsRequired)
        {
            isMagical = intIsMagical;
            isConsumable = intIsConsumable;
            bonusStrength = intBonusStrength;
            bonusHP = intBonusHP;
        }

        public override int GetBonus()
        {
            return bonusStrength;
        }

        public override bool GetIsConsumable()
        {
            return isConsumable;
        }

        public override bool IsEquipable()
        {
            return true; // Los objetos pueden ser equipados
        }

        public override bool HasEffects()
        {
            return bonusStrength > 0 || bonusHP > 0; // Tiene efectos si hay bonificaciones
        }

        // Se llama durante combate (efecto temporal)
        public override void Use(Hero targetHero, Enemy targetEnemy)
        {
            bool effectApplied = false;

            // Aplicar curación (temporal)
            if (bonusHP > 0)
            {
                int actualHP = targetHero.GetActualLife();
                int maxHP = targetHero.GetHP();

                if (actualHP < maxHP)
                {
                    int newHP = actualHP + bonusHP;
                    if (newHP > maxHP)
                    {
                        newHP = maxHP;
                    }
                    targetHero.SetActualHP(newHP);
                    Console.WriteLine(targetHero.GetName() + " ha recuperado " + (newHP - actualHP) + " puntos de vida.");
                    effectApplied = true;
                }
                else
                {
                    Console.WriteLine(targetHero.GetName() + " ya tiene la vida al máximo. No se recupera vida.");
                }
            }

            // Aplicar bonificación temporal de fuerza
            if (bonusStrength > 0)
            {
                targetHero.SetStrength(targetHero.GetStrength() + bonusStrength);
                Console.WriteLine(targetHero.GetName() + " gana +" + bonusStrength + " fuerza temporal.");
                effectApplied = true;
            }

            // Mostrar si el objeto no tuvo efecto
            if (!effectApplied)
            {
                Console.WriteLine("El objeto " + GetName() + " no tuvo ningún efecto.");
            }
        }

        // Se llama si se implementa mecánica de equipamiento
        public void Equip(Hero targetHero)
        {
            if (bonusStrength > 0)
            {
                targetHero.SetStrength(targetHero.GetStrength() + bonusStrength);
                Console.WriteLine(targetHero.GetName() + " equipa " + GetName() + " y gana +" + bonusStrength + " fuerza.");
            }

            if (bonusHP > 0)
            {
                targetHero.SetHP(targetHero.GetHP() + bonusHP);
                targetHero.SetActualHP(targetHero.GetActualLife() + bonusHP);
                Console.WriteLine(targetHero.GetName() + " equipa " + GetName() + " y gana +" + bonusHP + " HP permanente.");
            }
        }

        public override void ShowInfo()
        {
            Console.WriteLine("Nombre: " + GetName());
            Console.WriteLine("Tipo: Objeto");
            Console.WriteLine("Mágico: " + (isMagical ? "Sí" : "No"));
            Console.WriteLine("Consumible: " + (isConsumable ? "Sí" : "No"));
            Console.WriteLine("Manos requeridas: " + GetHandsRequired());
            Console.WriteLine("Bonificaciones -> Fuerza: " + bonusStrength + ", HP: " + bonusHP);
            Console.WriteLine("Compra: " + GetBuyValue() + " | Venta: " + GetSellValue());
        }
    }
    public class Spell : Item
    {
        int spellDifficulty;
        int bonusIntelligence;
        bool isConsumable;

        public Spell(string intName, int intBuyValue, int intSellValue, int intHandsRequired, int intDifficulty, int intBonusIntelligence)
            : base(intName, intBuyValue, intSellValue, intHandsRequired)
        {
            spellDifficulty = intDifficulty;
            bonusIntelligence = intBonusIntelligence;
            isConsumable = false; // Por norma, los hechizos no se consumen
        }

        public override int GetBonus()
        {
            return bonusIntelligence;
        }

        public override bool GetIsConsumable()
        {
            return isConsumable;
        }

        public override bool IsEquipable()
        {
            return false; // Los hechizos no son equipables
        }

        public override bool HasEffects()
        {
            return true; // Los hechizos pueden tener efectos
        }

        public override void Use(Hero targetHero, Enemy targetEnemy)
        {
            Random random = new Random();
            int roll = random.Next(1, 11);
            int totalIntelligence = targetHero.GetIntelligence() + bonusIntelligence;

            Console.WriteLine(targetHero.GetName() + " intenta lanzar " + GetName() + ". Tirada: " + roll + ", Dificultad: " + spellDifficulty);

            if ((roll + totalIntelligence) >= spellDifficulty)
            {
                Console.WriteLine("¡El hechizo se lanzó con éxito!");
                // Aquí puedes agregar lógica para aplicar efectos del hechizo si deseas
            }
            else
            {
                Console.WriteLine("El hechizo falló.");
            }
        }

        public override void ShowInfo()
        {
            Console.WriteLine("Nombre: " + GetName());
            Console.WriteLine("Tipo: Hechizo (siempre mágico)");
            Console.WriteLine("Consumible: No");
            Console.WriteLine("Manos requeridas: " + GetHandsRequired());
            Console.WriteLine("Bonificación de inteligencia: " + bonusIntelligence);
            Console.WriteLine("Dificultad: " + spellDifficulty);
            Console.WriteLine("Compra: " + GetBuyValue() + " | Venta: " + GetSellValue());
        }

        public override bool IsOffensive()
        {
            return true; // Asumimos todos los hechizos son ofensivos por defecto
        }
    }
    public class Inventory
    {
        List<Item> items;

        public Inventory()
        {
            items = new List<Item>();
        }

        public void AddItem(Item newItem)
        {
            items.Add(newItem);
        }

        public void RemoveItem(Item itemToRemove)
        {
            items.Remove(itemToRemove);
        }

        public int GetItemCount()
        {
            return items.Count;
        }

        public Item GetItem(int index)
        {
            if (index >= 0 && index < items.Count)
            {
                return items[index];
            }
            return null; // Retorna null si el índice está fuera de rango
        }

        public void ShowInventory()
        {
            Console.WriteLine("=== INVENTORY ===");
            for (int i = 0; i < items.Count; i++)
            {
                Console.WriteLine((i + 1) + ".");
                items[i].ShowInfo();
                Console.WriteLine();
            }
        }
    }

    public class Hero
    {
        // Atributos
        string name;
        string title;
        string race;
        string heroClass;
        string lore;
        DateTime recruitmentTime;
        int strength;
        int constitution;
        int hp;
        int intelligence;
        int speed;
        int actualHP;
        int level;
        int xp;
        List<Item> equippedItems = new List<Item>();

        // Constructor
        public Hero(string intName, string intTitle, string intRace, string intClass, string intLore, int intStrength, int intConstitution, int intHP, int intIntelligence, int intSpeed)
        {
            name = intName;
            title = intTitle;
            race = intRace;
            heroClass = intClass;
            lore = intLore;
            strength = intStrength;
            constitution = intConstitution;
            hp = intHP;
            intelligence = intIntelligence;
            speed = intSpeed;

            // Modificadores por raza
            switch (race)
            {
                case "Humano":
                    {
                        hp = hp + 1;
                        intelligence = intelligence + 1;
                        break;
                    }
                case "Elfo":
                    {
                        intelligence = intelligence + 2;
                        speed = speed + 1;
                        break;
                    }
                case "Orco":
                    {
                        strength = strength + 2;
                        constitution = constitution + 1;
                        break;
                    }
                case "Enano":
                    {
                        constitution = constitution + 2;
                        hp = hp + 1;
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            // Modificadores por clase
            switch (heroClass)
            {
                case "Guerrero":
                    {
                        strength = strength + 2;
                        constitution = constitution + 1;
                        break;
                    }
                case "Mago":
                    {
                        intelligence = intelligence + 2;
                        hp = hp + 1;
                        break;
                    }
                case "Sanador":
                    {
                        constitution = constitution + 2;
                        intelligence = intelligence + 1;
                        break;
                    }
                case "Ladrón":
                    {
                        speed = speed + 2;
                        strength = strength + 1;
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            actualHP = hp;
            level = 1;
            xp = 0;
            recruitmentTime = DateTime.Now;
        }

        // Setters
        public void SetName(string newName)
        {
            name = newName;
        }

        public void SetTitle(string newTitle)
        {
            title = newTitle;
        }

        public void SetRace(string newRace)
        {
            race = newRace;
        }

        public void SetClass(string newClass)
        {
            heroClass = newClass;
        }

        public void SetLore(string newLore)
        {
            lore = newLore;
        }

        public void SetStrength(int newStrength)
        {
            strength = newStrength;
        }

        public void SetConstitution(int newConstitution)
        {
            constitution = newConstitution;
        }

        public void SetHP(int newHP)
        {
            hp = newHP;
        }

        public void SetIntelligence(int newIntelligence)
        {
            intelligence = newIntelligence;
        }

        public void SetSpeed(int newSpeed)
        {
            speed = newSpeed;
        }

        public void SetActualHP(int newActualHP)
        {
            actualHP = newActualHP;
        }

        public void SetLevel(int newLevel)
        {
            level = newLevel;
        }

        public void SetXP(int newXP)
        {
            xp = newXP;
        }

        // Getters
        public string GetName()
        {
            return name;
        }

        public string GetTitle()
        {
            return title;
        }

        public string GetRace()
        {
            return race;
        }

        public string GetClass()
        {
            return heroClass;
        }

        public string GetLore()
        {
            return lore;
        }

        public DateTime GetRecruitmentTime()
        {
            return recruitmentTime;
        }

        public int GetStrength()
        {
            return strength;
        }

        public int GetConstitution()
        {
            return constitution;
        }

        public int GetHP()
        {
            return hp;
        }

        public int GetIntelligence()
        {
            return intelligence;
        }

        public int GetSpeed()
        {
            return speed;
        }

        public int GetActualLife()
        {
            return actualHP;
        }

        public int GetLevel()
        {
            return level;
        }

        public int GetXP()
        {
            return xp;
        }

        // Modificadores

        public void AddXP(int gainedXP)
        {
            xp = xp + gainedXP;

            while (xp >= 100)
            {
                xp = xp - 100;
                level = level + 1;

                int oldHP = hp;
                int oldStrength = strength;
                int oldSpeed = speed;

                hp = hp + 1;
                strength = strength + 2;
                speed = speed + 1;
                actualHP = hp;

                Console.WriteLine("¡" + name + " ha subido al nivel " + level + "!");
                Console.WriteLine("HP -> " + oldHP + " + 1");
                Console.WriteLine("Strength -> " + oldStrength + " + 2");
                Console.WriteLine("Speed -> " + oldSpeed + " + 1");
                Console.WriteLine("Constitution -> " + constitution);
                Console.WriteLine("Intelligence -> " + intelligence);
                Console.WriteLine("");
            }
        }

        public void EquipItem(Item item)
        {
            if (!equippedItems.Contains(item))
            {
                equippedItems.Add(item);
            }
        }

        public void RemoveItem(Item item)
        {
            if (equippedItems.Contains(item))
            {
                equippedItems.Remove(item);
            }
        }

        public List<Item> GetItems()
        {
            return equippedItems;
        }
    }
    public class Enemy
    {
        // Atributos
        string name;
        int resistance;
        int attack;
        int health;
        int speed;
        int money;
        int experience;
        bool physicalResistance;
        bool magicalImmunity;

        // Constructor
        public Enemy(string intName, int intResistance, int intAttack, int intHealth, int intSpeed, int intMoney, int intExperience, bool intPhysicalResistance, bool intMagicalImmunity)
        {
            name = intName;
            resistance = intResistance;
            attack = intAttack;
            health = intHealth;
            speed = intSpeed;
            money = intMoney;
            experience = intExperience;
            physicalResistance = intPhysicalResistance;
            magicalImmunity = intMagicalImmunity;
        }

        // Setters
        public void SetName(string newName)
        {
            name = newName;
        }

        public void SetResistance(int newResistance)
        {
            resistance = newResistance;
        }

        public void SetAttack(int newAttack)
        {
            attack = newAttack;
        }

        public void SetHealth(int newHealth)
        {
            health = newHealth;
        }

        public void SetSpeed(int newSpeed)
        {
            speed = newSpeed;
        }

        public void SetMoney(int newMoney)
        {
            money = newMoney;
        }

        public void SetExperience(int newExperience)
        {
            experience = newExperience;
        }

        public void SetPhysicalResistance(bool newPhysicalResistance)
        {
            physicalResistance = newPhysicalResistance;
        }

        public void SetMagicalImmunity(bool newMagicalImmunity)
        {
            magicalImmunity = newMagicalImmunity;
        }

        // Getters
        public string GetName()
        {
            return name;
        }

        public int GetResistance()
        {
            return resistance;
        }

        public int GetAttack()
        {
            return attack;
        }

        public int GetHealth()
        {
            return health;
        }

        public int GetSpeed()
        {
            return speed;
        }

        public int GetMoney()
        {
            return money;
        }

        public int GetExperience()
        {
            return experience;
        }

        public bool GetPhysicalResistance()
        {
            return physicalResistance;
        }

        public bool GetMagicalImmunity()
        {
            return magicalImmunity;
        }
    }
    
    public class HeroAttack
    {
        public Hero hero;
        public Enemy enemy;

        public HeroAttack(Hero h, Enemy e)
        {
            hero = h;
            enemy = e;
        }
    }
    public class CombatAction
    {
        public Hero hero;
        public bool attacked = false;
        public bool killed = false;
        public bool couldHaveKilledButMissed = false;
        public int xpExtra = 0;

        public CombatAction(Hero hero)
        {
            this.hero = hero;
        }
    }
    public class Equipacion
    {
        private string nombre;
        private List<Hero> heroes;
        private bool activa;

        public Equipacion(string nombre, List<Hero> heroes)
        {
            this.nombre = nombre;
            this.heroes = new List<Hero>(heroes);
            this.activa = false;
        }

        public string GetNombre()
        {
            return nombre;
        }

        public void SetNombre(string nuevoNombre)
        {
            nombre = nuevoNombre;
        }

        public List<Hero> GetHeroes()
        {
            return heroes;
        }

        public void SetHeroes(List<Hero> nuevosHeroes)
        {
            heroes = nuevosHeroes;
        }

        public bool GetActiva()
        {
            return activa;
        }

        public void SetActiva(bool estado)
        {
            activa = estado;
        }
    }

    public class Adventure
    {
        // Atributos
        string adventureName;
        string uniqueAdventureID;

        int playerGold;
        DateTime startDate;
        TimeSpan totalPlayTime;

        List<Hero> recruitedHeroes;
        List<Hero> activeParty;
        List<Equipacion> savedFormations;
        Inventory inventory;

        DateTime sessionStartTime;

        // Constructor
        public Adventure(string intName, int intRegionCode)
        {
            adventureName = intName;
            uniqueAdventureID = GenerateUniqueID(intRegionCode);
            playerGold = 50;
            startDate = DateTime.Now;
            totalPlayTime = new TimeSpan();

            recruitedHeroes = new List<Hero>();
            activeParty = new List<Hero>();
            savedFormations = new List<Equipacion>();
            inventory = new Inventory();

            sessionStartTime = DateTime.Now;
        }

        // --- Getters ---
        public string GetAdventureName()
        {
            return adventureName;
        }

        public string GetUniqueID()
        {
            return uniqueAdventureID;
        }

        public int GetGold()
        {
            return playerGold;
        }

        public DateTime GetStartDate()
        {
            return startDate;
        }

        public TimeSpan GetTotalPlayTime()
        {
            return totalPlayTime;
        }

        public List<Hero> GetHeroes()
        {
            return recruitedHeroes;
        }

        public List<Hero> GetActiveParty()
        {
            return activeParty;
        }

        public List<Equipacion> GetFormations()
        {
            return savedFormations;
        }

        public Inventory GetInventory()
        {
            return inventory;
        }

        // --- Setters y modificadores ---
        public void AddGold(int intAmount)
        {
            playerGold = playerGold + intAmount;
        }

        public void SpendGold(int intAmount)
        {
            playerGold = playerGold - intAmount;
        }

        public void AddHero(Hero intHero)
        {
            if (recruitedHeroes.Count < 10)
            {
                recruitedHeroes.Add(intHero);
            }
        }

        public void RemoveHero(Hero intHero)
        {
            recruitedHeroes.Remove(intHero);
            activeParty.Remove(intHero);
        }

        public void AddFormation(Equipacion intFormation)
        {
            if (savedFormations.Count < 10)
            {
                savedFormations.Add(intFormation);
            }
        }

        // --- Funciones auxiliares ---
        public void StartSession()
        {
            sessionStartTime = DateTime.Now;
        }

        public void EndSession()
        {
            TimeSpan played = DateTime.Now - sessionStartTime;
            totalPlayTime = totalPlayTime + played;
        }

        public string GenerateUniqueID(int intRegion)
        {
            Random rng = new Random();
            int number = rng.Next(100000000, 999999999);
            string id = intRegion.ToString() + number.ToString();
            return id;
        }
    }

    public class BattleManager
    {
        Random rng = new Random();

        public int Combat(List<Hero> heroes, List<Enemy> enemies, int currentGold, Inventory inventory)
        {
            Console.WriteLine("¡Comienza el combate!");

            if (heroes.Count == 0 || enemies.Count == 0)
            {
                Console.WriteLine("No hay héroes o enemigos para combatir.");
                return currentGold;
            }

            bool combatActive = true;

            while (combatActive)
            {
                // Turno de héroes
                for (int i = 0; i < heroes.Count; i++)
                {
                    Hero hero = heroes[i];
                    if (hero.GetActualLife() <= 0) continue;

                    Console.WriteLine("\nTurno de héroe: " + hero.GetName() + " (HP: " + hero.GetActualLife() + "/" + hero.GetHP() + ")");
                    Enemy selectedEnemy = SelectEnemy(enemies);

                    UseItemOrAttack(hero, selectedEnemy, inventory);

                    if (selectedEnemy.GetHealth() <= 0)
                    {
                        Console.WriteLine(selectedEnemy.GetHealth() + " ha sido derrotado!");
                        enemies.Remove(selectedEnemy);
                    }
                    if (enemies.Count == 0)
                    {
                        Console.WriteLine("¡Todos los enemigos han sido derrotados!");
                        combatActive = false;
                        break;
                    }
                }

                // Turno de enemigos
                for (int j = 0; j < enemies.Count; j++)
                {
                    Enemy enemy = enemies[j];
                    if (enemy.GetHealth() <= 0) continue;

                    Hero targetHero = heroes[rng.Next(heroes.Count)];
                    Console.WriteLine(enemy.GetHealth() + " ataca a " + targetHero.GetName() + ".");

                    int damage = enemy.GetAttack();
                    int newHP = targetHero.GetActualLife() - damage;
                    if (newHP < 0) newHP = 0;
                    targetHero.SetActualHP(newHP);

                    Console.WriteLine(targetHero.GetName() + " recibe " + damage + " de daño. HP restante: " + targetHero.GetActualLife());

                    if (targetHero.GetActualLife() <= 0)
                    {
                        Console.WriteLine(targetHero.GetName() + " ha sido derrotado!");
                        heroes.Remove(targetHero);
                    }
                    if (heroes.Count == 0)
                    {
                        Console.WriteLine("¡Todos los héroes han caído! Fin del combate.");
                        combatActive = false;
                        break;
                    }
                }
            }

            return currentGold;
        }

        public Enemy SelectEnemy(List<Enemy> enemies)
        {
            Console.WriteLine("Selecciona un enemigo para atacar:");
            for (int i = 0; i < enemies.Count; i++)
            {
                Console.WriteLine((i + 1) + ". " + enemies[i].GetName() + " (HP: " + enemies[i].GetHealth() + ")");
            }

            int selectedIndex = -1;
            while (selectedIndex < 0 || selectedIndex >= enemies.Count)
            {
                Console.Write("Opción: ");
                string input = Console.ReadLine();
                bool parsed = int.TryParse(input, out selectedIndex);
                if (parsed)
                {
                    selectedIndex--; // Ajustar índice para lista
                }
                else
                {
                    Console.WriteLine("Selección inválida.");
                    selectedIndex = -1;
                }
            }

            return enemies[selectedIndex];
        }

        public void UseItemOrAttack(Hero hero, Enemy enemy, Inventory inventory)
        {
            Console.WriteLine("¿Quieres usar un objeto o atacar? (1: Usar objeto, 2: Atacar)");
            string choice = Console.ReadLine();

            if (choice == "1")
            {
                inventory.ShowInventory();
                Console.Write("Selecciona un objeto (número): ");
                string input = Console.ReadLine();
                int itemIndex;
                bool parsed = int.TryParse(input, out itemIndex);
                itemIndex = itemIndex - 1; // Ajustar

                if (parsed && itemIndex >= 0 && itemIndex < inventory.GetItemCount())
                {
                    Item item = inventory.GetItem(itemIndex);
                    item.Use(hero, enemy);
                    if (item.GetIsConsumable())
                    {
                        inventory.RemoveItem(item);
                    }
                }
                else
                {
                    Console.WriteLine("Selección inválida, pasando al ataque básico.");
                    BasicAttack(hero, enemy);
                }
            }
            else if (choice == "2")
            {
                BasicAttack(hero, enemy);
            }
            else
            {
                Console.WriteLine("Opción inválida, pasando al ataque básico.");
                BasicAttack(hero, enemy);
            }
        }

        public void BasicAttack(Hero hero, Enemy enemy)
        {
            int damage = hero.GetStrength() - enemy.GetResistance();
            if (damage < 1)
            {
                damage = 1;
            }

            int vidaNueva = enemy.GetHealth() - damage;
            if (vidaNueva < 0)
            {
                vidaNueva = 0;
            }
            enemy.SetHealth(vidaNueva);
            Console.WriteLine(hero.GetName() + " ataca a " + enemy.GetName() + " y causa " + damage + " de daño. Vida restante del enemigo: " + enemy.GetHealth());
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            // Crear aventura
            Adventure aventuraActual = new Adventure("MiAventura", 0); // Región 0 = Europa
            aventuraActual.StartSession();

            // Crear héroes y añadirlos a la aventura
            Hero h1 = new Hero("Thalendir", "El Escudo", "Humano", "Guerrero", "Capitán", 5, 3, 5, 2, 4);
            Hero h2 = new Hero("Elandra", "La Dama", "Elfa", "Maga", "Guardiana", 4, 2, 4, 5, 6);
            Hero h3 = new Hero("Gorak", "Puño de Hierro", "Orco", "Sanador", "Chamán", 3, 5, 6, 2, 3);

            aventuraActual.AddHero(h1);
            aventuraActual.AddHero(h2);
            aventuraActual.AddHero(h3);

            // Crear objetos y hechizos
            Inventory gameInventory = aventuraActual.GetInventory();
            InitializeInventory(gameInventory);

            // Preparación de menú
            bool playing = true;
            Random rng = new Random();
            List<Equipacion> equipaciones = new List<Equipacion>();

            Console.WriteLine("¡Bienvenido aventurero!\n");

            while (playing)
            {
                Console.WriteLine("Oro: " + aventuraActual.GetGold());
                Console.WriteLine("¿Qué deseas hacer?");
                Console.WriteLine("1. Plaza del pueblo");
                Console.WriteLine("2. Ir a Mazmorra");
                Console.WriteLine("3. Curar en Taberna");
                Console.WriteLine("4. Bazar (pendiente)");
                Console.WriteLine("5. Contrabandista (pendiente)");
                Console.WriteLine("6. Escuela de magia (pendiente)");
                Console.WriteLine("7. Guardar aventura (pendiente)");
                Console.WriteLine("8. Cargar aventura (pendiente)");
                Console.WriteLine("9. Abandonar la aventura");
                Console.Write("Selección: ");
                string option = Console.ReadLine();
                Console.WriteLine("");

                switch (option)
                {
                    case "1":
                        TownSquare(aventuraActual, equipaciones);
                        break;

                    case "2":
                        if (aventuraActual.GetActiveParty().Count == 0)
                        {
                            Console.WriteLine("No hay equipación activa. Ve a la Plaza del Pueblo para preparar una.");
                        }
                        else
                        {
                            Console.WriteLine("¡Expedición lista! Pulsa ENTER para comenzar la aventura.");
                            Console.ReadLine();

                            int nuevoOro = Dungeon(aventuraActual.GetActiveParty(), aventuraActual.GetGold(), rng, aventuraActual.GetHeroes(), aventuraActual.GetInventory());
                            aventuraActual.SpendGold(aventuraActual.GetGold()); // Reset
                            aventuraActual.AddGold(nuevoOro);
                        }
                        break;

                    case "3":
                        int oroRestante = Healer(aventuraActual.GetGold(), aventuraActual.GetHeroes());
                        aventuraActual.SpendGold(aventuraActual.GetGold());
                        aventuraActual.AddGold(oroRestante);
                        break;

                    case "4": // Bazar
                        Console.WriteLine("Opción Bazar seleccionada - funcionalidad pendiente.");
                        break;

                    case "5": // Contrabandista
                        Console.WriteLine("Opción Contrabandista seleccionada - funcionalidad pendiente.");
                        break;

                    case "6": // Escuela de magia
                        Console.WriteLine("Opción Escuela de Magia seleccionada - funcionalidad pendiente.");
                        break;

                    case "7": // Guardar aventura
                        Console.WriteLine("Opción Guardar aventura seleccionada - funcionalidad pendiente.");
                        break;

                    case "8": // Cargar aventura
                        Console.WriteLine("Opción Cargar aventura seleccionada - funcionalidad pendiente.");
                        break;

                    case "9": // Abandonar la aventura
                        Console.WriteLine("Se perderán todos los datos no guardados. ¿Desea continuar? (s/n)");
                        string confirm = Console.ReadLine().ToLower();
                        if (confirm == "s")
                        {
                            Console.WriteLine("¡Gracias por jugar aventurero!");
                            playing = false;
                            aventuraActual.EndSession();
                        }
                        else
                        {
                            Console.WriteLine("Volviendo al menú principal...");
                        }
                        break;

                    default:
                        Console.WriteLine("Opción inválida.\n");
                        break;
                }
            }
        }
        static void InitializeInventory(Inventory inventory)
        {
            // Objetos comunes (Físicos)
            inventory.AddItem(new Object("Espada de Acero", 20, 10, 1, false, false, 2, 0));
            inventory.AddItem(new Object("Espada Roma", 5, 1, 1, false, false, 1, 0));
            inventory.AddItem(new Object("Escudo de Madera", 15, 8, 1, false, false, 0, 2));

            // Objetos comunes (Mágicos)
            inventory.AddItem(new Object("Escudo Mágico", 50, 25, 1, true, false, 0, 3));
            inventory.AddItem(new Object("Poción de Curación", 10, 5, 0, false, true, 0, 2));
            inventory.AddItem(new Object("Poción de Constitución", 15, 5, 0, true, true, 0, 1));

            // Objetos únicos (Físicos)
            inventory.AddItem(new Object("Espada Gigante", 500, 100, 2, false, false, 6, 0));
            inventory.AddItem(new Object("Daga Oscura", 700, 600, 1, false, false, 4, 0));

            // Objetos únicos (Mágicos)
            inventory.AddItem(new Object("Espada Legendaria", 1000, 90, 2, true, false, 6, 0));
            inventory.AddItem(new Object("Escudo de la Eternidad", 160, 80, 2, true, false, 0, 4));

            // Hechizos
            inventory.AddItem(new Spell("Hechizo de Fuego", 30, 10, 0, 2, 3));
            inventory.AddItem(new Spell("Hechizo de Fortalecimiento", 25, 8, 1, 1, 2));
            inventory.AddItem(new Spell("Hechizo de Escudo Mental", 30, 10, 0, 2, 2));
            inventory.AddItem(new Spell("Hechizo de Curación Mayor", 40, 15, 1, 3, 4));
        }
        static void TownSquare(Adventure adventure, List<Equipacion> equipaciones)
        {
            bool inPlaza = true;

            while (inPlaza)
            {
                Console.WriteLine("=== PLAZA DEL PUEBLO ===");
                Console.WriteLine("(1) Ver todos los héroes");
                Console.WriteLine("(2) Equipaciones de héroes");
                Console.WriteLine("(3) Inventario");
                Console.WriteLine("(4) Salir");
                Console.Write("Selecciona una opción: ");
                string plazaOption = Console.ReadLine();
                Console.WriteLine();

                switch (plazaOption)
                {
                    case "1":
                        {
                            Console.WriteLine("Héroes Reclutados:\n");

                            List<Hero> allHeroes = adventure.GetHeroes();

                            for (int counterHero = 0; counterHero < allHeroes.Count; counterHero++)
                            {
                                Hero h = allHeroes[counterHero];
                                Console.WriteLine(h.GetName());
                                Console.WriteLine("|");
                                Console.WriteLine("Título: " + h.GetTitle());
                                Console.WriteLine("Raza: " + h.GetRace());
                                Console.WriteLine("Clase: " + h.GetClass());
                                Console.WriteLine("Lore: " + h.GetLore());
                                Console.WriteLine("Reclutado el: " + h.GetRecruitmentTime());
                                Console.WriteLine("Vida: " + h.GetActualLife() + "/" + h.GetHP());
                                Console.WriteLine("Fuerza: " + h.GetStrength());
                                Console.WriteLine("Constitución: " + h.GetConstitution());
                                Console.WriteLine("Inteligencia: " + h.GetIntelligence());
                                Console.WriteLine("Velocidad: " + h.GetSpeed());
                                Console.WriteLine("Nivel: " + h.GetLevel());
                                Console.WriteLine("Experiencia: " + h.GetXP() + "/100");
                                Console.WriteLine();
                            }

                            break;
                        }

                    case "2":
                        {
                            List<Hero> allHeroes = adventure.GetHeroes();
                            List<Hero> activeParty = adventure.GetActiveParty();

                            EquipationsMenu(adventure.GetHeroes(), adventure.GetActiveParty(), equipaciones);
                            break;
                        }

                    case "3":
                        {
                            adventure.GetInventory().ShowInventory();
                            break;
                        }

                    case "4":
                        {
                            inPlaza = false;
                            break;
                        }

                    default:
                        {
                            Console.WriteLine("Opción no válida.");
                            break;
                        }
                }

                Console.WriteLine();
            }
        }
        static void EquipationsMenu(List<Hero> allHeroes, List<Hero> activeParty, List<Equipacion> equipaciones)
        {
            bool inEquipaciones = true;

            while (inEquipaciones)
            {
                Console.WriteLine("=== EQUIPACIONES ===");

                if (equipaciones.Count < 5)
                {
                    Console.WriteLine("(1) Crear nueva equipación");
                }

                Console.WriteLine("(2) Ver equipaciones");
                Console.WriteLine("(3) Eliminar equipación");
                Console.WriteLine("(4) Salir");
                Console.Write("Selecciona una opción: ");
                string option = Console.ReadLine();
                Console.WriteLine();

                switch (option)
                {
                    case "1":
                        {
                            if (equipaciones.Count >= 5)
                            {
                                Console.WriteLine("Ya tienes el máximo de 5 equipaciones.");
                                break;
                            }

                            List<Hero> nuevaLista = new List<Hero>();
                            Console.WriteLine("Selecciona hasta 4 héroes para esta equipación:");

                            for (int counterHero = 0; counterHero < allHeroes.Count; counterHero++)
                            {
                                Console.WriteLine((counterHero + 1) + ". " + allHeroes[counterHero].GetName());
                            }

                            while (nuevaLista.Count < 4 && nuevaLista.Count < allHeroes.Count)
                            {
                                Console.Write("Selecciona un número de héroe: ");
                                string input = Console.ReadLine();
                                int seleccion;
                                bool valido = int.TryParse(input, out seleccion);

                                if (valido && seleccion >= 1 && seleccion <= allHeroes.Count)
                                {
                                    Hero elegido = allHeroes[seleccion - 1];
                                    if (!nuevaLista.Contains(elegido))
                                    {
                                        nuevaLista.Add(elegido);
                                        Console.WriteLine(elegido.GetName() + " añadido.");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Ese héroe ya está en la lista.");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Selección inválida.");
                                }
                            }

                            Console.Write("Introduce un nombre para esta equipación: ");
                            string nombreEquipacion = Console.ReadLine();

                            equipaciones.Add(new Equipacion(nombreEquipacion, nuevaLista));
                            Console.WriteLine("Equipación creada con éxito.");
                            break;
                        }

                    case "2":
                        {
                            Console.WriteLine("Equipaciones disponibles:");
                            for (int counterEquip = 0; counterEquip < equipaciones.Count; counterEquip++)
                            {
                                string activa = equipaciones[counterEquip].GetActiva() ? " (Activa)" : "";
                                Console.WriteLine((counterEquip + 1) + " - " + equipaciones[counterEquip].GetNombre() + activa);
                            }

                            Console.WriteLine((equipaciones.Count + 1) + " - Salir");
                            Console.Write("Selecciona una equipación para ver más opciones: ");
                            string input = Console.ReadLine();
                            int seleccion;
                            bool valido = int.TryParse(input, out seleccion);

                            if (valido && seleccion >= 1 && seleccion <= equipaciones.Count)
                            {
                                Equipacion elegida = equipaciones[seleccion - 1];
                                bool enDetalle = true;

                                while (enDetalle)
                                {
                                    Console.WriteLine("\n=== " + elegida.GetNombre() + " ===");
                                    Console.WriteLine("(1) Ver héroes");
                                    Console.WriteLine("(2) Activar esta equipación");
                                    Console.WriteLine("(3) Volver");
                                    Console.Write("Selecciona una opción: ");
                                    string opcionDetalle = Console.ReadLine();

                                    switch (opcionDetalle)
                                    {
                                        case "1":
                                            List<Hero> heroes = elegida.GetHeroes();
                                            for (int i = 0; i < heroes.Count; i++)
                                            {
                                                Console.WriteLine(heroes[i].GetName());
                                            }
                                            break;

                                        case "2":
                                            for (int i = 0; i < equipaciones.Count; i++)
                                            {
                                                equipaciones[i].SetActiva(false);
                                            }
                                            elegida.SetActiva(true);

                                            activeParty.Clear();
                                            List<Hero> nuevaParty = elegida.GetHeroes();
                                            for (int i = 0; i < nuevaParty.Count; i++)
                                            {
                                                activeParty.Add(nuevaParty[i]);
                                            }

                                            Console.WriteLine("Equipación activada.");
                                            break;

                                        case "3":
                                            enDetalle = false;
                                            break;

                                        default:
                                            Console.WriteLine("Opción no válida.");
                                            break;
                                    }
                                }
                            }

                            break;
                        }

                    case "3":
                        {
                            Console.WriteLine("¿Qué equipación deseas eliminar?");
                            for (int i = 0; i < equipaciones.Count; i++)
                            {
                                Console.WriteLine((i + 1) + " - " + equipaciones[i].GetNombre());
                            }

                            Console.Write("Introduce el número de la equipación: ");
                            string input = Console.ReadLine();
                            int seleccion;
                            bool valido = int.TryParse(input, out seleccion);

                            if (valido && seleccion >= 1 && seleccion <= equipaciones.Count)
                            {
                                string nombre = equipaciones[seleccion - 1].GetNombre();
                                Console.Write("¿Realmente quieres eliminar la equipación \"" + nombre + "\"? (s/n): ");
                                string confirmacion = Console.ReadLine();
                                if (confirmacion.ToLower() == "s")
                                {
                                    equipaciones.RemoveAt(seleccion - 1);
                                    Console.WriteLine("Equipación eliminada.");
                                }
                                else
                                {
                                    Console.WriteLine("Cancelado.");
                                }
                            }

                            break;
                        }

                    case "4":
                        {
                            inEquipaciones = false;
                            break;
                        }

                    default:
                        {
                            Console.WriteLine("Opción inválida.");
                            break;
                        }
                }

                Console.WriteLine();
            }
        }
        static int Healer(int totalGold, List<Hero> aliveHeroes)
        {
            if (aliveHeroes.Count == 0)
            {
                Console.WriteLine("No tienes héroes vivos para curar.");
                Console.WriteLine("");
            }
            else
            {
                int totalHealingCost = 0;
                int counterHeroesToCheck = 0;

                while (counterHeroesToCheck < aliveHeroes.Count)
                {
                    Hero heroToCheck = aliveHeroes[counterHeroesToCheck];
                    int maxHealth = heroToCheck.GetHP();
                    int currentHealth = heroToCheck.GetActualLife();

                    if (currentHealth < maxHealth)
                    {
                        int missingPercentage = (maxHealth - currentHealth) * 100 / maxHealth;
                        int healingCost = (missingPercentage / 20) * 2;

                        if (healingCost == 0)
                        {
                            healingCost = 2;
                        }

                        totalHealingCost = totalHealingCost + healingCost;
                    }

                    counterHeroesToCheck = counterHeroesToCheck + 1;
                }

                if (totalHealingCost > 0)
                {
                    if (totalGold >= totalHealingCost)
                    {
                        totalGold = totalGold - totalHealingCost;

                        Console.WriteLine();
                        Console.WriteLine("Coste total de curación: " + totalHealingCost + " de oro.");
                        Console.WriteLine("Te quedan: " + totalGold + " de oro.");
                        Console.WriteLine();

                        int counterHealHeroes = 0;
                        while (counterHealHeroes < aliveHeroes.Count)
                        {
                            Hero heroToHeal = aliveHeroes[counterHealHeroes];
                            if (heroToHeal.GetActualLife() < heroToHeal.GetHP())
                            {
                                heroToHeal.SetActualHP(heroToHeal.GetHP());
                            }
                            counterHealHeroes = counterHealHeroes + 1;
                        }

                        Console.WriteLine("¡Héroes curados correctamente!");
                        Console.WriteLine();
                    }
                    else
                    {
                        Console.WriteLine("Coste total de curación: " + totalHealingCost + " de oro.");
                        Console.WriteLine("Te quedan: " + totalGold + " de oro.");
                        Console.WriteLine("");
                        Console.WriteLine("No tienes suficiente oro para curar.");
                        Console.WriteLine("GAME OVER.");
                        totalGold = 0;
                    }
                }
                else
                {
                    Console.WriteLine("Todos tus héroes ya están con la vida al máximo.");
                }
            }

            return totalGold;
        }
        static int Dungeon(List<Hero> group, int totalGold, Random rng, List<Hero> aliveHeroes, Inventory gameInventory)
        {
            List<List<Enemy>> enemyWaves = new List<List<Enemy>>();

            enemyWaves.Add(new List<Enemy>{new Enemy("Gólem", 3, 2, 5, 3, 5, 15, true, true),new Enemy("Esqueleto", 2, 1, 3, 4, 3, 10, false, false)});

            enemyWaves.Add(new List<Enemy> {new Enemy("Orco", 4, 2, 6, 3, 7, 20, true, true), new Enemy("Mago Oscuro", 3, 3, 4, 5, 6, 25, false, false)});

            int initialGold = totalGold;
            Dictionary<Hero, int> originalStrength = new Dictionary<Hero, int>();
            foreach (Hero hero in group)
            {
                originalStrength[hero] = hero.GetStrength();
            }

            foreach (List<Enemy> enemies in enemyWaves)
            {
                bool isBattleActive = true;
                List<CombatAction> actions = group.Select(h => new CombatAction(h)).ToList();

                while (isBattleActive)
                {
                    List<HeroAttack> pendingAttacks = new List<HeroAttack>();
                    List<Item> usedItems = new List<Item>();

                    foreach (Hero currentHero in group.ToList())
                    {
                        if (enemies.Count == 0) break;

                        bool turnInProgress = true;
                        while (turnInProgress)
                        {
                            Console.WriteLine("Turno de: " + currentHero.GetName());
                            Console.WriteLine("1. Atacar");
                            Console.WriteLine("2. Usar Objeto");
                            Console.WriteLine("3. Pasar Turno");
                            Console.Write("Selecciona una opción: ");
                            string choice = Console.ReadLine();
                            Console.WriteLine();

                            if (choice == "1")
                            {
                                Console.WriteLine("Elige un monstruo para atacar:");
                                for (int i = 0; i < enemies.Count; i++)
                                {
                                    Console.WriteLine($"{i + 1}. {enemies[i].GetName()} (Vida: {enemies[i].GetHealth()})");
                                }

                                Console.Write("Selecciona: ");
                                if (int.TryParse(Console.ReadLine(), out int selectedEnemyIndex) &&
                                    selectedEnemyIndex >= 1 && selectedEnemyIndex <= enemies.Count)
                                {
                                    pendingAttacks.Add(new HeroAttack(currentHero, enemies[selectedEnemyIndex - 1]));
                                    turnInProgress = false;
                                }
                                else
                                {
                                    Console.WriteLine("Selección inválida.\n");
                                }
                            }
                            else if (choice == "2")
                            {
                                gameInventory.ShowInventory();
                                Console.Write("Introduce el número del objeto que quieres usar: ");
                                if (int.TryParse(Console.ReadLine(), out int selectedItemIndex) &&
                                    selectedItemIndex >= 1 && selectedItemIndex <= gameInventory.GetItemCount())
                                {
                                    Item selectedItem = gameInventory.GetItem(selectedItemIndex - 1);

                                    if (selectedItem is Spell)
                                    {
                                        Console.WriteLine("Selecciona enemigo:");
                                        for (int i = 0; i < enemies.Count; i++)
                                            Console.WriteLine($"{i + 1}. {enemies[i].GetName()}");

                                        if (int.TryParse(Console.ReadLine(), out int enemyTargetIndex) &&
                                            enemyTargetIndex >= 1 && enemyTargetIndex <= enemies.Count)
                                        {
                                            selectedItem.Use(currentHero, enemies[enemyTargetIndex - 1]);
                                        }
                                        else
                                        {
                                            Console.WriteLine("Objetivo inválido.\n");
                                            continue;
                                        }
                                    }
                                    else
                                    {
                                        selectedItem.Use(currentHero, null);
                                    }

                                    if (selectedItem.GetIsConsumable())
                                        gameInventory.RemoveItem(selectedItem);

                                    turnInProgress = false;
                                }
                                else
                                {
                                    Console.WriteLine("Selección inválida.\n");
                                }
                            }
                            else if (choice == "3")
                            {
                                turnInProgress = false;
                            }
                            else
                            {
                                Console.WriteLine("Opción inválida.\n");
                            }
                        }
                    }

                    pendingAttacks.Sort((a, b) => b.hero.GetSpeed().CompareTo(a.hero.GetSpeed()));

                    foreach (var attack in pendingAttacks)
                    {
                        Hero hero = attack.hero;
                        Enemy enemy = attack.enemy;

                        if (!enemies.Contains(enemy)) continue;

                        Console.WriteLine($"{hero.GetName()} ataca a {enemy.GetName()}");
                        int roll = rng.Next(1, 11);
                        Console.WriteLine("Tirada: " + roll);
                        int attackPower = hero.GetStrength();

                        if (roll == 10)
                        {
                            Console.WriteLine("¡Golpe crítico!");
                            attackPower = (int)(attackPower * 1.25);
                        }

                        if (roll >= 6)
                        {
                            if (attackPower >= enemy.GetHealth())
                            {
                                Console.WriteLine("¡" + enemy.GetName() + " ha sido derrotado!");
                                totalGold += enemy.GetMoney();
                                enemies.Remove(enemy);

                                var action = actions.Find(a => a.hero == hero);
                                action.killed = true;
                                action.xpExtra += enemy.GetExperience();
                            }
                            else
                            {
                                Console.WriteLine("No fue suficiente para derrotarlo.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Falló el ataque.");
                        }
                        Console.WriteLine();
                    }

                    // Mostrar los próximos 6 turnos
                    List<object> combinedTurnOrder = new List<object>();
                    combinedTurnOrder.AddRange(group);
                    combinedTurnOrder.AddRange(enemies);
                    combinedTurnOrder.Sort((a, b) =>
                    {
                        int speedA = a is Hero ? ((Hero)a).GetSpeed() : ((Enemy)a).GetSpeed();
                        int speedB = b is Hero ? ((Hero)b).GetSpeed() : ((Enemy)b).GetSpeed();
                        return speedB.CompareTo(speedA);
                    });

                    Console.WriteLine("Próximos 6 turnos:");
                    for (int i = 0; i < Math.Min(6, combinedTurnOrder.Count); i++)
                    {
                        if (combinedTurnOrder[i] is Hero h)
                            Console.WriteLine($"{i + 1}. {h.GetName()}");
                        else if (combinedTurnOrder[i] is Enemy e)
                            Console.WriteLine($"{i + 1}. {e.GetName()}");
                    }
                    Console.WriteLine();

                    // Ataque de enemigos (considerando doble ataque si son mucho más rápidos)
                    foreach (Enemy enemy in enemies.ToList())
                    {
                        if (group.Count == 0) break;

                        Hero target = group[rng.Next(group.Count)];
                        int attacks = enemy.GetSpeed() - target.GetSpeed() >= 5 ? 2 : 1;

                        if (attacks == 2)
                            Console.WriteLine($"{enemy.GetName()} es mucho más rápido que {target.GetName()} y ataca DOS veces.");

                        for (int i = 0; i < attacks; i++)
                        {
                            int damage = enemy.GetAttack();
                            int remainingHP = target.GetActualLife() - damage;

                            Console.WriteLine($"{enemy.GetName()} ataca a {target.GetName()} y causa {damage} de daño.");

                            if (remainingHP <= 0)
                            {
                                Console.WriteLine($"{target.GetName()} ha sido derrotado.");
                                aliveHeroes.Remove(target);
                                group.Remove(target);
                                break;
                            }
                            else
                            {
                                target.SetActualHP(remainingHP);
                                Console.WriteLine($"Vida restante de {target.GetName()}: {remainingHP}/{target.GetHP()}");
                            }

                            Console.WriteLine();
                        }
                    }

                    if (enemies.Count == 0)
                    {
                        Console.WriteLine("La oleada ha sido derrotada.");
                        isBattleActive = false;

                        int goldEarned = totalGold - initialGold;
                        if (goldEarned < 0) goldEarned = 0;

                        Console.WriteLine("Resumen del combate:");
                        Console.WriteLine("Oro ganado: " + goldEarned);
                        Console.WriteLine("Oro total actual: " + totalGold);

                        foreach (var acc in actions)
                        {
                            int xpGained = acc.killed ? acc.xpExtra : 5;

                            if (xpGained == 0)
                                Console.WriteLine($"{acc.hero.GetName()} no participó y no gana XP.");
                            else
                            {
                                acc.hero.AddXP(xpGained);
                                Console.WriteLine($"{acc.hero.GetName()} gana {xpGained} XP.");
                            }
                        }

                        Console.WriteLine();
                    }

                    if (group.Count == 0)
                    {
                        Console.WriteLine("¡Todos los héroes han caído!");
                        isBattleActive = false;
                        totalGold = Healer(totalGold, aliveHeroes);
                    }
                }
            }

            foreach (Hero h in group)
            {
                if (h.GetActualLife() > h.GetHP())
                    h.SetActualHP(h.GetHP());

                if (originalStrength.ContainsKey(h))
                    h.SetStrength(originalStrength[h]);
            }

            return totalGold;
        }
    }
}
