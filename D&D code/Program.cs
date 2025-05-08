using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


namespace D_D_code
{
    [Serializable]
    public abstract class Item
    {
        // Atributos protegidos
        protected string itemName;
        protected int itemBuyValue;
        protected int itemSellValue;
        protected int itemHandsRequired;
        protected bool isConsumable;  // Nuevo atributo para indicar si es consumible o no

        // Constructor
        public Item(string intName, int intBuyValue, int intSellValue, int intHandsRequired, bool intIsConsumable)
        {
            itemName = intName;
            itemBuyValue = intBuyValue;
            itemSellValue = intSellValue;
            itemHandsRequired = intHandsRequired;
            isConsumable = intIsConsumable;  // Inicialización del parámetro isConsumable
        }

        // Getters
        public string GetName() { return itemName; }
        public int GetBuyValue() { return itemBuyValue; }
        public int GetSellValue() { return itemSellValue; }
        public int GetHandsRequired() { return itemHandsRequired; }
        public virtual bool GetIsConsumable() { return isConsumable; }  // Método virtual para obtener si es consumible

        // Métodos modificadores
        public virtual bool IsOffensive() { return false; }
        public virtual bool IsEquipable() { return false; }  // Por defecto, no es equipable
        public virtual bool HasEffects() { return false; }  // Por defecto, no tiene efectos

        // Métodos abstractos
        public abstract int GetBonus();
        public abstract void Use(Hero targetHero, Enemy targetEnemy);
        public abstract void ShowInfo();

        // Métodos abstractos para obtener bonificaciones
        public abstract int GetStrengthBonus();
        public abstract int GetConstitutionBonus();
        public abstract int GetSpeedBonus();
        public abstract int GetHpBonus();
    }

    [Serializable]
    public class Object : Item
    {
        private bool isMagical;
        private int bonusStrength;
        private int bonusConstitution;
        private int bonusIntelligence;
        private int bonusSpeed;
        private int bonusHP;
        private int tier;

        // Constructor y otros métodos

        public Object(string intName, int intBuyValue, int intSellValue, int intHandsRequired, bool intIsMagical,
                      int intBonusStrength, int intBonusConstitution, int intBonusIntelligence, int intBonusSpeed,
                      int intBonusHP, int intTier)
            : base(intName, intBuyValue, intSellValue, intHandsRequired, false)
        {
            isMagical = intIsMagical;
            bonusStrength = intBonusStrength;
            bonusConstitution = intBonusConstitution;
            bonusIntelligence = intBonusIntelligence;
            bonusSpeed = intBonusSpeed;
            bonusHP = intBonusHP;
            tier = intTier;
        }


        // Implementación del método abstracto GetBonus()
        public override int GetBonus()
        {
            // Devuelve la suma de todas las bonificaciones, o una bonificación específica
            // Dependiendo de cómo quieras que funcione este método, puedes sumar todas las bonificaciones o devolver solo una.
            // Ejemplo: Devolver la bonificación de fuerza
            return bonusStrength + bonusConstitution + bonusIntelligence + bonusSpeed + bonusHP;
        }

        public override int GetStrengthBonus() { return bonusStrength; }
        public override int GetConstitutionBonus() { return bonusConstitution; }
        public override int GetSpeedBonus() { return bonusSpeed; }
        public override int GetHpBonus() { return bonusHP; }
        public int GetTier()
        {
            return tier;
        }


        public override void Use(Hero targetHero, Enemy targetEnemy)
        {
            // Lógica de uso, según el tipo de objeto
            Console.WriteLine($"{targetHero.GetName()} usa el objeto {GetName()}.");
        }

        public override void ShowInfo()
        {
            Console.WriteLine("Nombre: " + GetName());
            Console.WriteLine("Consumible: " + GetIsConsumable());
            Console.WriteLine("Manos requeridas: " + GetHandsRequired());
            Console.WriteLine("Fuerza: " + bonusStrength);
            Console.WriteLine("Constitución: " + bonusConstitution);
            Console.WriteLine("Inteligencia: " + bonusIntelligence);
            Console.WriteLine("Velocidad: " + bonusSpeed);
            Console.WriteLine("HP: " + bonusHP);
            Console.WriteLine("Compra: " + GetBuyValue() + " | Venta: " + GetSellValue());
        }

        public override bool IsOffensive() { return false; }
    }
    
    [Serializable]
    public class Spell : Item
    {
        int spellDifficulty;
        int bonusIntelligence;

        public Spell(string intName, int intBuyValue, int intSellValue, int intHandsRequired, int intDifficulty, int intBonusIntelligence)
            : base(intName, intBuyValue, intSellValue, intHandsRequired, false)  // Los hechizos no son consumibles por defecto
        {
            spellDifficulty = intDifficulty;
            bonusIntelligence = intBonusIntelligence;
        }

        public override int GetBonus() { return bonusIntelligence; }

        public override bool GetIsConsumable() { return isConsumable; }

        public override bool IsEquipable() { return false; }

        public override bool HasEffects() { return true; }

        // Métodos abstractos implementados
        public override int GetStrengthBonus() { return 0; }  // Hechizos no tienen bonificación de fuerza
        public override int GetConstitutionBonus() { return 0; }  // Hechizos no tienen bonificación de constitución
        public override int GetSpeedBonus() { return 0; }  // Hechizos no tienen bonificación de velocidad
        public override int GetHpBonus() { return 0; }  // Hechizos no tienen bonificación de HP

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

        public override bool IsOffensive() { return true; }  // Asumimos todos los hechizos son ofensivos por defecto
    }
    
    [Serializable]
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

        // Método para obtener todos los objetos en el inventario
        public List<Item> GetItems()
        {
            return items; // Devuelve la lista completa de objetos
        }

        public Item GetItem(int index)
        {
            if (index >= 0 && index < items.Count)
            {
                return items[index];
            }
            return null; // Retorna null si el índice está fuera de rango
        }

        public Item ShowInventory(string where)
        {
            Console.WriteLine("=== INVENTORY ===");
            for (int i = 0; i < items.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {items[i].GetName()}");
            }

            Console.WriteLine("[0] Volver al menú...");

            switch (where)
            {
                case "TownSquare":
                    {
                        Console.Write("Selecciona un objeto para ver detalles o equipar: ");
                        string input = Console.ReadLine();

                        if (int.TryParse(input, out int selectedIndex) && selectedIndex >= 1 && selectedIndex <= items.Count)
                        {
                            Item selectedItem = items[selectedIndex - 1];
                            Console.WriteLine($"Seleccionaste: {selectedItem.GetName()}");

                            Console.WriteLine("¿Qué deseas hacer?");
                            Console.WriteLine("1. Ver detalles del objeto");
                            Console.WriteLine("2. Equipar objeto");
                            Console.WriteLine("0. Volver al inventario");
                            Console.Write("Selecciona una opción: ");
                            string action = Console.ReadLine();

                            switch (action)
                            {
                                case "1":
                                    selectedItem.ShowInfo(); // Muestra la información del objeto
                                    break;

                                case "2":
                                    return selectedItem; // Devolverá el item para que el que llama lo equipe
                                case "0":
                                    Console.WriteLine("Volviendo al inventario...");
                                    break;
                                default:
                                    Console.WriteLine("Opción no válida.");
                                    break;
                            }
                        }
                        else if (input == "0")
                        {
                            Console.WriteLine("Volviendo al menú...");
                        }
                        break;
                    }
                case "Dungeon":
                    {
                        break;
                    }
            }

            return null; // Si no se equipó nada
        }
    }

    [Serializable]
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
        Item equippedItem;

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

        public Item GetEquippedItem()
        {
            return equippedItem; // Devuelve el ítem actualmente equipado, o null si no hay ninguno.
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

        public void RemoveItem()
        {
            if (equippedItem != null)
            {
                // Remover las estadísticas del héroe basadas en el ítem
                strength -= equippedItem.GetStrengthBonus();
                constitution -= equippedItem.GetConstitutionBonus();
                hp -= equippedItem.GetHpBonus();
                speed -= equippedItem.GetSpeedBonus();
                actualHP -= equippedItem.GetHpBonus(); // Ajustamos la vida real

                equippedItem = null;
                Console.WriteLine($"{name} ya no tiene ningún objeto equipado.");
            }
            else
            {
                Console.WriteLine($"{name} no tiene ningún objeto equipado.");
            }
        }

        public void SetItem(Item item)
        {
            // Verificar si el héroe ya tiene un objeto equipado
            if (equippedItem != null)
            {
                Console.WriteLine($"{name} ya tiene el objeto {equippedItem.GetName()} equipado.");
                Console.WriteLine($"¿Deseas reemplazar el objeto {equippedItem.GetName()} con {item.GetName()}? (s/n)");
                string respuesta = Console.ReadLine().ToLower();

                if (respuesta == "s")
                {
                    // Remover las bonificaciones del ítem anterior
                    strength -= equippedItem.GetStrengthBonus();
                    constitution -= equippedItem.GetConstitutionBonus();
                    hp -= equippedItem.GetHpBonus();
                    speed -= equippedItem.GetSpeedBonus();
                    actualHP -= equippedItem.GetHpBonus(); // Ajustamos la vida real

                    // Equipar el nuevo ítem
                    equippedItem = item;

                    // Agregar las bonificaciones del nuevo ítem
                    strength += item.GetStrengthBonus();
                    constitution += item.GetConstitutionBonus();
                    hp += item.GetHpBonus();
                    speed += item.GetSpeedBonus();
                    actualHP += item.GetHpBonus(); // Aseguramos que se actualice la vida real

                    Console.WriteLine($"{item.GetName()} ha sido equipado a {name}.");
                }
                else
                {
                    Console.WriteLine("El objeto no ha sido equipado.");
                }
            }
            else
            {
                // Si no tiene un ítem equipado, lo equipamos directamente
                equippedItem = item;

                // Agregar las bonificaciones del ítem
                strength += item.GetStrengthBonus();
                constitution += item.GetConstitutionBonus();
                hp += item.GetHpBonus();
                speed += item.GetSpeedBonus();
                actualHP += item.GetHpBonus(); // Aseguramos que se actualice la vida real

                Console.WriteLine($"{item.GetName()} ha sido equipado a {name}.");
            }
        }

    }
    
    [Serializable]
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

    [Serializable]
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

    [Serializable]
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
    
    [Serializable]
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

        public string GetName()
        {
            return nombre;
        }

        public void SetName(string nuevoNombre)
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

        public bool GetActive()
        {
            return activa;
        }

        public void SetActive(bool estado)
        {
            activa = estado;
        }
    }

    [Serializable]
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

    [Serializable]
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
                inventory.ShowInventory("TownSquare");
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

    [Serializable]
    public static class UtilidadesTienda
    {
        public static List<Object> FiltrarObjetosPorTipo(List<Item> todos, bool soloUnicos, bool soloComunes)
        {
            return todos.OfType<Object>()
                .Where(o =>
                {
                    bool esUnico = o.GetBuyValue() >= 100; // Puedes ajustar este criterio
                    if (soloUnicos) return esUnico;
                    if (soloComunes) return !esUnico;
                    return true;
                }).ToList();
        }

        public static List<Spell> ObtenerHechizos(List<Item> todos)
        {
            return todos.OfType<Spell>().ToList();
        }
    }

    [Serializable]
    public class Bazar
    {
        public List<Item> objetosDisponibles = new List<Item>();

        public void Actualizar(List<Object> objetosComunes)
        {
            objetosDisponibles.Clear();

            if (objetosComunes == null || objetosComunes.Count == 0)
            {
                return;
            }

            Random rng = new Random();
            for (int i = 0; i < 3; i++)
            {
                int index = rng.Next(objetosComunes.Count);
                objetosDisponibles.Add(objetosComunes[index]);
            }
        }

        public void MostrarYComprar(Adventure aventura, List<Object> poolObjetosComunes)
        {
            if (objetosDisponibles.Count == 0)
            {
                Actualizar(poolObjetosComunes);
            }

            Console.WriteLine("\n=== BAZAR ===");
            Console.WriteLine($"Tu oro actual: {aventura.GetGold()}\n");
            for (int i = 0; i < objetosDisponibles.Count; i++)
            {
                var obj = objetosDisponibles[i];
                Console.WriteLine($"[{i + 1}] {obj.GetName()} - {obj.GetBuyValue()} oro");
            }
            Console.WriteLine("[0] Volver");

            Console.Write("Selecciona un objeto para comprar: ");
            if (int.TryParse(Console.ReadLine(), out int eleccion) && eleccion >= 1 && eleccion <= objetosDisponibles.Count)
            {
                var seleccionado = objetosDisponibles[eleccion - 1];
                if (aventura.GetGold() >= seleccionado.GetBuyValue())
                {
                    aventura.SpendGold(seleccionado.GetBuyValue());
                    aventura.GetInventory().AddItem(seleccionado);
                    objetosDisponibles.RemoveAt(eleccion - 1);
                    Console.WriteLine($"Has comprado {seleccionado.GetName()}.");
                }
                else
                {
                    Console.WriteLine("No tienes suficiente oro.");
                }
            }
        }

    }

    [Serializable]
    public class Contrabandista
    {
        public void Interactuar(Adventure aventura, List<Object> objetosUnicos)
        {
            if (aventura.GetGold() < 10)
            {
                Console.WriteLine("No tienes suficiente oro para entrar al Contrabandista (10).");
                return;
            }

            aventura.SpendGold(10);
            Console.WriteLine("\nHas pagado 10 de oro para acceder al Contrabandista.");
            Console.WriteLine($"Tu oro actual: {aventura.GetGold()}\n");

            List<Object> inventarioActual = aventura.GetInventory().GetItems().OfType<Object>().ToList();
            List<string> nombresInventario = inventarioActual.Select(o => o.GetName()).ToList();
            List<Object> unicosFiltrados = objetosUnicos
                .Where(o => !nombresInventario.Contains(o.GetName()))
                .GroupBy(o => o.GetName()).Select(g => g.First()).ToList();

            if (unicosFiltrados.Count == 0)
            {
                Console.WriteLine("No quedan objetos únicos disponibles para ofrecerte.");
                return;
            }

            Random rng = new Random();
            List<Object> oferta = unicosFiltrados.OrderBy(_ => rng.Next()).Take(3).ToList();

            for (int i = 0; i < oferta.Count; i++)
            {
                Console.WriteLine($"[{i + 1}] {oferta[i].GetName()} - {oferta[i].GetBuyValue()} oro");
            }
            Console.WriteLine("[0] No comprar nada");

            Console.Write("Selecciona uno para comprar (solo uno): ");
            if (int.TryParse(Console.ReadLine(), out int eleccion) && eleccion >= 1 && eleccion <= oferta.Count)
            {
                Object elegido = oferta[eleccion - 1];
                if (aventura.GetGold() >= elegido.GetBuyValue())
                {
                    aventura.SpendGold(elegido.GetBuyValue());
                    aventura.GetInventory().AddItem(elegido);
                    Console.WriteLine($"Has comprado {elegido.GetName()}.");
                }
                else
                {
                    Console.WriteLine("No tienes suficiente oro.");
                }
            }

            Console.WriteLine("El contrabandista desaparece en la sombra...");
        }
    }

    [Serializable]
    public class EscuelaMagia
    {
        private bool accesoPagado = false;
        private bool hechizoComprado = false;
        private List<Spell> hechizos = new List<Spell>();

        public void Actualizar(List<Spell> pool)
        {
            Random rng = new Random();
            hechizos = pool.OrderBy(_ => rng.Next()).Take(3).ToList();
            accesoPagado = false;
            hechizoComprado = false;
        }

        public void Interactuar(Adventure aventura, List<Spell> hechizosDisponibles)
        { 
            if (!accesoPagado)
            {
                Console.WriteLine("¿Deseas pagar 15 de oro para acceder a la Escuela de Magia? (s/n)");
                if (Console.ReadLine().ToLower() == "s")
                {
                    if (aventura.GetGold() >= 15)
                    {
                        aventura.SpendGold(15);
                        this.hechizos = hechizosDisponibles.OrderBy(x => new Random().Next()).Take(3).ToList();
                    accesoPagado = true;
                        hechizoComprado = false;
                        Console.WriteLine("Has pagado por acceder a la Escuela de Magia.");
                    }
                    else
                    {
                        Console.WriteLine("No tienes suficiente oro.");
                        return;
                    }
                }
                else return;
        }


            Console.WriteLine("\n=== ESCUELA DE MAGIA ===");
            Console.WriteLine($"Tu oro actual: {aventura.GetGold()}\n");

            for (int i = 0; i < hechizos.Count; i++)
            {
                Console.WriteLine($"[{i + 1}] {hechizos[i].GetName()} - {hechizos[i].GetBuyValue()} oro");
            }
            Console.WriteLine("[0] Volver");

            if (hechizoComprado)
            {
                Console.WriteLine("Ya has comprado un hechizo. Solo puedes observar ahora.");
                return;
            }

            Console.Write("Selecciona un hechizo para comprar: ");
            if (int.TryParse(Console.ReadLine(), out int eleccion) && eleccion >= 1 && eleccion <= hechizos.Count)
            {
                Spell seleccionado = hechizos[eleccion - 1];
                if (aventura.GetGold() >= seleccionado.GetBuyValue())
                {
                    aventura.SpendGold(seleccionado.GetBuyValue());
                    aventura.GetInventory().AddItem(seleccionado);
                    hechizoComprado = true;
                    Console.WriteLine($"Has aprendido {seleccionado.GetName()}.");
                }
                else
                {
                    Console.WriteLine("No tienes suficiente oro.");
                }
            }
        }
    }

    internal class Program
    {

        /*
        Funcionalidades restantes:
        
        Crear el personaje al principio despues de cargar y antes del menu
        
        Errores:
        
        La curacion de revivivr a los heroes debe costar 10 por cabeza
        Al tener game over en la dungeon el juego debe finalizar
        
        Pequeñas cosas utiles:
        
        Cuando seleccionas nueva partida y la partida ya esta creada en ese slot seleccionado debe preguntar si quieres sobreescribir
        Si ya hay un slot de partida con ese nombre decirle que no se puede crear la partida porque ya hay una con ese mismo nombre
        Cuando el objeto equipado en un personaje es una pocion de curacion el personaje bebera la pocion cuando el personaje baje de la mitad de la vida sin gastar turno
        
        Cosas que estaban en la practica y no lei:
        
        Ampliar el numero de slots de equipaciones a 10
        Modificar tras crear las equipaciones
        Poder "vender" heroes pero nunca te debes quedar con menos de 1
        Se pueden vender items
        Que se ofrezca al jugador la posibilidad de elegir entre varios grupos enemigos antes de comenzar el combate
        Personajes con mayor velocidad actuarán con mayor frecuencia, incluso pudiendo repetir turno
        Para cada hechizo se debe realizar una tirada de Inteligencia
        Se calcula la cantidad de dados de combate con la siguiente formula
        Fuerza final del héroe - Resistencia del monstruo = número de dados a lanzar.
        Se lanza esa cantidad de dados.
        Al subir nivel eliges las estadisticas a subir
        Añadir opcion en plaza del pubelo que indique los datos tecnicos de la aventura tales como el id, el numero de horas y la fecha de creacion
        */

        static void Main(string[] args)
        {
            const int MAX_SLOTS = 3;
            string[] slots = new string[MAX_SLOTS];
            string[] rutasSlots = new string[MAX_SLOTS];

            for (int i = 0; i < MAX_SLOTS; i++)
            {
                rutasSlots[i] = $"slot{i + 1}.dat";
                slots[i] = File.Exists(rutasSlots[i]) ? ObtenerNombreAventura(rutasSlots[i]) : "----------";
            }

            Adventure aventuraActual = null;
            int slotSeleccionado = -1;

            while (true)
            {
                Console.WriteLine("=== MENÚ INICIAL ===");
                Console.WriteLine("1. Nueva partida");
                Console.WriteLine("2. Cargar partida");
                Console.WriteLine("3. Salir");
                Console.Write("Selecciona una opción: ");
                string opcionInicio = Console.ReadLine();

                if (opcionInicio == "1")
                {
                    Console.WriteLine("\nElige un slot de guardado (1-3):");
                    MostrarSlots(slots);

                    if (int.TryParse(Console.ReadLine(), out slotSeleccionado) && slotSeleccionado >= 1 && slotSeleccionado <= MAX_SLOTS)
                    {
                        Console.Write("Escribe el nombre de tu aventura: ");
                        string nombreAventura = Console.ReadLine();
                        aventuraActual = new Adventure(nombreAventura, 0);
                        aventuraActual.StartSession();

                        // Crear héroes
                        aventuraActual.AddHero(new Hero("Thalendir", "El Escudo", "Humano", "Guerrero", "Capitán", 5, 3, 5, 2, 4));
                        aventuraActual.AddHero(new Hero("Elandra", "La Dama", "Elfa", "Maga", "Guardiana", 4, 2, 4, 5, 6));
                        aventuraActual.AddHero(new Hero("Gorak", "Puño de Hierro", "Orco", "Sanador", "Chamán", 3, 5, 6, 2, 3));

                        GuardarAventura(aventuraActual, rutasSlots[slotSeleccionado - 1]);
                        break;
                    }
                }
                else if (opcionInicio == "2")
                {
                    Console.WriteLine("\nPartidas guardadas:");
                    MostrarSlots(slots);
                    Console.Write("Selecciona un slot (1-3): ");

                    if (int.TryParse(Console.ReadLine(), out slotSeleccionado) && slotSeleccionado >= 1 && slotSeleccionado <= MAX_SLOTS)
                    {
                        string ruta = rutasSlots[slotSeleccionado - 1];

                        if (File.Exists(ruta))
                        {
                            aventuraActual = CargarAventura(ruta);
                            aventuraActual.StartSession();
                        }
                        else
                        {
                            Console.WriteLine("No hay partida en ese slot. Se iniciará una nueva.");
                            Console.Write("Nombre de la nueva aventura: ");
                            string nombreAventura = Console.ReadLine();
                            aventuraActual = new Adventure(nombreAventura, 0);
                            aventuraActual.StartSession();

                            aventuraActual.AddHero(new Hero("Thalendir", "El Escudo", "Humano", "Guerrero", "Capitán", 5, 3, 5, 2, 4));
                            aventuraActual.AddHero(new Hero("Elandra", "La Dama", "Elfa", "Maga", "Guardiana", 4, 2, 4, 5, 6));
                            aventuraActual.AddHero(new Hero("Gorak", "Puño de Hierro", "Orco", "Sanador", "Chamán", 3, 5, 6, 2, 3));
                            GuardarAventura(aventuraActual, ruta);
                        }
                        break;
                    }
                }
                else if (opcionInicio == "3")
                {
                    Console.WriteLine("¡Hasta la próxima!");
                    return;
                }
            }

            // --- CONTINÚA JUEGO NORMAL ---
            Inventory gameInventory = new Inventory();
            InitializeInventory(gameInventory);

            Inventory jugadorInventory = aventuraActual.GetInventory();
            List<Equipacion> equipaciones = aventuraActual.GetFormations();
            Random rng = new Random();
            Bazar bazar = new Bazar();
            Contrabandista contrabandista = new Contrabandista();
            EscuelaMagia escuela = new EscuelaMagia();
            bool playing = true;

            Console.WriteLine($"\n¡Bienvenido a {aventuraActual.GetAdventureName()}!\n");

            while (playing)
            {
                Console.WriteLine("Oro: " + aventuraActual.GetGold());
                Console.WriteLine("¿Qué deseas hacer?");
                Console.WriteLine("1. Plaza del pueblo");
                Console.WriteLine("2. Ir a Mazmorra");
                Console.WriteLine("3. Curar en Taberna");
                Console.WriteLine("4. Bazar");
                Console.WriteLine("5. Contrabandista");
                Console.WriteLine("6. Escuela de magia");
                Console.WriteLine("7. Guardar partida");
                Console.WriteLine("0. Abandonar la aventura");
                Console.Write("Selección: ");
                string option = Console.ReadLine();
                Console.WriteLine();

                switch (option)
                {
                    case "1":
                        TownSquare(aventuraActual, equipaciones);
                        GuardarAventura(aventuraActual, rutasSlots[slotSeleccionado - 1]); // AUTOGUARDADO
                        break;

                    case "2":
                        if (aventuraActual.GetActiveParty().Count == 0)
                        {
                            Console.WriteLine("No hay equipación activa.");
                        }
                        else
                        {
                            Console.WriteLine("¡Expedición lista! Pulsa ENTER para comenzar la aventura.");
                            Console.ReadLine();

                            int nuevoOro = Dungeon(aventuraActual.GetActiveParty(), aventuraActual.GetGold(), rng, aventuraActual.GetHeroes(), aventuraActual.GetInventory(), aventuraActual, bazar, escuela);
                            aventuraActual.SpendGold(aventuraActual.GetGold());
                            aventuraActual.AddGold(nuevoOro);
                            GuardarAventura(aventuraActual, rutasSlots[slotSeleccionado - 1]); // AUTOGUARDADO
                        }
                        break;

                    case "3":
                        bool enTaberna = true;
                        while (enTaberna)
                        {
                            Console.WriteLine("\n--- TABERNA ---");
                            Console.WriteLine("¿A quién deseas curar?");
                            Console.WriteLine("1. Curar equipación");
                            Console.WriteLine("2. Todos los héroes");
                            Console.WriteLine("3. Seleccionar héroes manualmente");
                            Console.WriteLine("4. Salir de la taberna");
                            Console.Write("Opción: ");
                            string eleccion = Console.ReadLine();

                            List<Hero> seleccionados = new List<Hero>();

                            switch (eleccion)
                            {
                                case "1": // Curar equipación
                                    if (equipaciones.Count == 0)
                                    {
                                        Console.WriteLine("No hay equipaciones disponibles.");
                                        break;
                                    }

                                    Console.WriteLine("Selecciona una equipación para curar:");
                                    for (int i = 0; i < equipaciones.Count; i++)
                                    {
                                        string nombreEq = equipaciones[i].GetName();
                                        string nombresHeroes = string.Join(", ", equipaciones[i].GetHeroes().Select(h => h.GetName()));
                                        Console.WriteLine($"[{i + 1}] {nombreEq} ({nombresHeroes})");
                                    }
                                    Console.WriteLine("[0] Volver");

                                    Console.Write("Opción: ");
                                    string inputEq = Console.ReadLine();
                                    if (inputEq == "0") break;

                                    if (int.TryParse(inputEq, out int idxEq) && idxEq >= 1 && idxEq <= equipaciones.Count)
                                    {
                                        seleccionados.AddRange(equipaciones[idxEq - 1].GetHeroes());
                                    }
                                    else
                                    {
                                        Console.WriteLine("Equipación no válida.");
                                    }
                                    break;

                                case "2": // Todos los héroes
                                    seleccionados.AddRange(aventuraActual.GetHeroes());
                                    break;

                                case "3": // Selección manual
                                    List<Hero> todos = aventuraActual.GetHeroes();
                                    for (int i = 0; i < todos.Count; i++)
                                    {
                                        Console.WriteLine($"{i + 1}. {todos[i].GetName()}");
                                    }

                                    Console.WriteLine("Introduce los números de los héroes separados por comas (o escribe 0 para volver):");
                                    string input = Console.ReadLine();
                                    if (input.Trim() == "0") break;

                                    string[] indices = input.Split(',');

                                    foreach (string s in indices)
                                    {
                                        if (int.TryParse(s.Trim(), out int num) && num >= 1 && num <= todos.Count)
                                        {
                                            seleccionados.Add(todos[num - 1]);
                                        }
                                        else
                                        {
                                            Console.WriteLine($"Número inválido: {s}");
                                        }
                                    }
                                    break;

                                case "4": // Salir de la taberna
                                    enTaberna = false;
                                    break;

                                default:
                                    Console.WriteLine("Opción no válida.");
                                    break;
                            }

                            if (seleccionados.Count > 0)
                            {
                                int oroRestante = Healer(aventuraActual.GetGold(), seleccionados);
                                aventuraActual.SpendGold(aventuraActual.GetGold());
                                aventuraActual.AddGold(oroRestante);
                            }
                        }
                        GuardarAventura(aventuraActual, rutasSlots[slotSeleccionado - 1]); // AUTOGUARDADO
                        break;

                    case "4":
                        var comunes = gameInventory.GetItems().OfType<Object>().Where(o => o.GetTier() == 1).ToList();
                        bazar.MostrarYComprar(aventuraActual, comunes);
                        GuardarAventura(aventuraActual, rutasSlots[slotSeleccionado - 1]); // AUTOGUARDADO
                        break;

                    case "5":
                        var unicos = gameInventory.GetItems().OfType<Object>().Where(o => o.GetTier() == 2).ToList();
                        contrabandista.Interactuar(aventuraActual, unicos);
                        GuardarAventura(aventuraActual, rutasSlots[slotSeleccionado - 1]); // AUTOGUARDADO
                        break;

                    case "6":
                        var hechizos = gameInventory.GetItems().OfType<Spell>().ToList();
                        escuela.Interactuar(aventuraActual, hechizos);
                        GuardarAventura(aventuraActual, rutasSlots[slotSeleccionado - 1]); // AUTOGUARDADO
                        break;

                    case "7":
                        {
                            GuardarAventura(aventuraActual, rutasSlots[slotSeleccionado - 1]); // AUTOGUARDADO
                            break;
                        }

                    case "0":
                        Console.WriteLine("¿Estás seguro de que quieres salir? (s/n)");
                        if (Console.ReadLine()?.ToLower() == "s")
                        {
                            Console.WriteLine("¡Hasta la próxima, aventurero!");
                            playing = false;
                            aventuraActual.EndSession();
                        }
                        break;

                    default:
                        Console.WriteLine("Opción no válida.\n");
                        break;
                }
            }
        }
        static void InitializeInventory(Inventory inventory)
        {
            // Objetos comunes (Físicos)
            inventory.AddItem(new Object("Espada de Acero", 20, 10, 1, false, 2, 0, 0, 0, 0, 1));  // tier = 1
            inventory.AddItem(new Object("Espada Roma", 5, 1, 1, false, 1, 0, 0, 0, 0, 1));
            inventory.AddItem(new Object("Escudo de Madera", 15, 8, 1, false, 0, 2, 0, 0, 2, 1));

            // Objetos comunes (Mágicos)
            inventory.AddItem(new Object("Escudo Mágico", 50, 25, 1, true, 0, 3, 0, 0, 2, 1));
            inventory.AddItem(new Object("Poción de Curación", 10, 5, 0, false, 0, 0, 0, 0, 2, 1));
            inventory.AddItem(new Object("Poción de Constitución", 15, 5, 0, true, 0, 1, 0, 0, 0, 1));

            // Objetos únicos (Físicos)
            inventory.AddItem(new Object("Espada Gigante", 500, 100, 2, false, 6, 0, 0, 0, 0, 2));
            inventory.AddItem(new Object("Daga Oscura", 700, 600, 1, false, 4, 0, 0, 0, 0, 2));

            // Objetos únicos (Mágicos)
            inventory.AddItem(new Object("Espada Legendaria", 1000, 90, 2, true, 6, 0, 0, 0, 0, 2));
            inventory.AddItem(new Object("Escudo de la Eternidad", 160, 80, 2, true, 0, 4, 0, 0, 0, 2));

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
                                Console.WriteLine("Vida: " + h.GetActualLife() + "/" + h.GetHP()); // Asegúrate de usar GetActualLife para la vida actual
                                Console.WriteLine("Fuerza: " + h.GetStrength());
                                Console.WriteLine("Constitución: " + h.GetConstitution());
                                Console.WriteLine("Inteligencia: " + h.GetIntelligence());
                                Console.WriteLine("Velocidad: " + h.GetSpeed());
                                Console.WriteLine("Nivel: " + h.GetLevel());
                                Console.WriteLine("Experiencia: " + h.GetXP() + "/100");

                                // Aquí es donde verificamos si el héroe tiene un objeto equipado
                                if (h.GetEquippedItem() != null) // Si tiene un ítem equipado
                                {
                                    Console.WriteLine($"Objeto equipado: {h.GetEquippedItem().GetName()}");
                                }

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

                    case "3": // Menú de inventario
                        Console.WriteLine("=== INVENTARIO ===");

                        // Mostrar objetos disponibles
                        List<Item> objetos = adventure.GetInventory().GetItems();
                        if (objetos.Count == 0)
                        {
                            Console.WriteLine("No tienes objetos en el inventario.");
                        }
                        else
                        {
                            Console.WriteLine("=== OBJETOS DISPONIBLES ===");
                            for (int i = 0; i < objetos.Count; i++)
                            {
                                Console.WriteLine($"{i + 1}. {objetos[i].GetName()}");
                            }
                            Console.WriteLine("[0] Volver al menú principal.");
                            Console.Write("Selecciona el objeto que deseas ver o equipar: ");
                            string input = Console.ReadLine();

                            if (int.TryParse(input, out int objetoIndex) && objetoIndex >= 1 && objetoIndex <= objetos.Count)
                            {
                                Item objetoSeleccionado = objetos[objetoIndex - 1];

                                // Preguntar qué desea hacer con el objeto
                                Console.WriteLine("¿Qué deseas hacer con este objeto?");
                                Console.WriteLine("1. Ver detalles");
                                Console.WriteLine("2. Equipar objeto");
                                Console.WriteLine("0. Volver al inventario");
                                string action = Console.ReadLine();

                                switch (action)
                                {
                                    case "1":
                                        objetoSeleccionado.ShowInfo(); // Mostrar información del objeto
                                        break;

                                    case "2":
                                        // Mostrar lista de héroes para seleccionar a quién equipar
                                        List<Hero> heroes = adventure.GetHeroes();
                                        Console.WriteLine("Selecciona el héroe al que deseas equipar el objeto:");
                                        for (int i = 0; i < heroes.Count; i++)
                                        {
                                            Console.WriteLine($"{i + 1}. {heroes[i].GetName()}");
                                        }

                                        Console.Write("Selecciona el héroe: ");
                                        string inputHero = Console.ReadLine();
                                        if (int.TryParse(inputHero, out int heroIndex) && heroIndex >= 1 && heroIndex <= heroes.Count)
                                        {
                                            Hero selectedHero = heroes[heroIndex - 1];

                                            // Usamos el método SetItem para equipar el objeto
                                            selectedHero.SetItem(objetoSeleccionado);
                                            selectedHero.GetStrength();
                                        }
                                        break;

                                    case "0":
                                        Console.WriteLine("Volviendo al inventario...");
                                        break;

                                    default:
                                        Console.WriteLine("Opción no válida.");
                                        break;
                                }
                            }
                            else if (input == "0")
                            {
                                Console.WriteLine("Volviendo al menú...");
                            }
                        }
                        break;

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
                                string activa = equipaciones[counterEquip].GetActive() ? " (Activa)" : "";
                                Console.WriteLine((counterEquip + 1) + " - " + equipaciones[counterEquip].GetName() + activa);
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
                                    Console.WriteLine("\n=== " + elegida.GetName() + " ===");
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
                                                equipaciones[i].SetActive(false);
                                            }
                                            elegida.SetActive(true);

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
                                string activa = equipaciones[i].GetActive() ? " (Activa)" : "";
                                Console.WriteLine((i + 1) + " - " + equipaciones[i].GetName() + activa);
                            }

                            Console.Write("Introduce el número de la equipación: ");
                            string input = Console.ReadLine();
                            int seleccion;
                            bool valido = int.TryParse(input, out seleccion);

                            if (valido && seleccion >= 1 && seleccion <= equipaciones.Count)
                            {
                                Equipacion equipacionAEliminar = equipaciones[seleccion - 1];
                                string nombre = equipacionAEliminar.GetName();

                                if (equipacionAEliminar.GetActive())
                                {
                                    Console.Write($"La equipación \"{nombre}\" está activa. ¿Seguro que quieres eliminarla? Esto desactivará la equipación. (s/n): ");
                                    string confirmacionActiva = Console.ReadLine();

                                    if (confirmacionActiva.ToLower() == "s")
                                    {
                                        equipaciones.RemoveAt(seleccion - 1);
                                        activeParty.Clear(); // Quitar la party activa también
                                        Console.WriteLine("Equipación activa eliminada. Ya no hay ninguna equipación activa.");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Cancelado.");
                                    }
                                }
                                else
                                {
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
                            }
                            else
                            {
                                Console.WriteLine("Selección inválida.");
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
            bool anyRevived = false;
            bool anyHealed = false;
            int totalHealingCost = 0;

            int deadCount = aliveHeroes.Count(h => h.GetActualLife() <= 0);
            bool allWereDead = deadCount == aliveHeroes.Count && deadCount > 0;

            foreach (Hero hero in aliveHeroes)
            {
                int maxHealth = hero.GetHP();
                int currentHealth = hero.GetActualLife();

                if (currentHealth <= 0)
                {
                    totalHealingCost += 10;
                    anyRevived = true;
                }
                else if (currentHealth < maxHealth)
                {
                    int missingPercentage = (maxHealth - currentHealth) * 100 / maxHealth;
                    int healingCost = (missingPercentage / 20) * 2;
                    if (healingCost == 0)
                        healingCost = 2;

                    totalHealingCost += healingCost;
                    anyHealed = true;
                }
            }

            if (totalHealingCost == 0)
            {
                Console.WriteLine("Todos tus héroes ya están con la vida al máximo.");
                return totalGold;
            }

            if (totalGold >= totalHealingCost)
            {
                totalGold -= totalHealingCost;

                foreach (Hero hero in aliveHeroes)
                {
                    hero.SetActualHP(hero.GetHP());
                }

                Console.WriteLine();
                Console.WriteLine("Coste total de curación: " + totalHealingCost + " de oro.");
                Console.WriteLine("Te quedan: " + totalGold + " de oro.");
                Console.WriteLine();

                if (allWereDead)
                    Console.WriteLine("Todos los héroes han sido revividos. Que no vuelva a ocurrir.");
                else if (anyRevived)
                    Console.WriteLine("¡Tus personajes fueron revividos!");
                else if (anyHealed)
                    Console.WriteLine("¡Tus personajes fueron curados con éxito!");

                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("Coste total de curación: " + totalHealingCost + " de oro.");
                Console.WriteLine("Te quedan: " + totalGold + " de oro.");
                Console.WriteLine();
                Console.WriteLine("No tienes suficiente oro para curar.");
                Console.WriteLine("GAME OVER.");
                totalGold = 0;
            }

            return totalGold;
        }
        static int Dungeon(List<Hero> activeParty, int totalGold, Random rng, List<Hero> allHeroes, Inventory gameInventory, Adventure aventuraActual, Bazar bazar, EscuelaMagia escuela)
        {
            List<Hero> group = activeParty.ToList(); // Trabajamos con copia para no tocar la equipación original
            List<List<Enemy>> enemyWaves = new List<List<Enemy>>();

            enemyWaves.Add(new List<Enemy> { new Enemy("Gólem", 3, 2, 5, 3, 5, 15, true, true), new Enemy("Esqueleto", 2, 1, 3, 4, 3, 10, false, false) });
            enemyWaves.Add(new List<Enemy> { new Enemy("Orco", 4, 2, 6, 3, 7, 20, true, true), new Enemy("Mago Oscuro", 3, 3, 4, 5, 6, 25, false, false) });

            int initialGold = totalGold;
            Dictionary<Hero, int> originalStrength = group.ToDictionary(hero => hero, hero => hero.GetStrength());

            foreach (List<Enemy> enemies in enemyWaves)
            {
                bool isBattleActive = true;
                List<CombatAction> actions = group.Select(h => new CombatAction(h)).ToList();

                while (isBattleActive)
                {
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

                    List<HeroAttack> pendingAttacks = new List<HeroAttack>();

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
                                    Console.WriteLine($"{i + 1}. {enemies[i].GetName()} (Vida: {enemies[i].GetHealth()})");

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
                                gameInventory.ShowInventory("Dungeon");
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

                    // Ataques de héroes
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
                                Console.WriteLine($"¡{enemy.GetName()} ha sido derrotado!");
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

                    // Ataques enemigos
                    foreach (Enemy enemy in enemies.ToList())
                    {
                        if (group.Count == 0) break;

                        Hero target = group[rng.Next(group.Count)];
                        int attacks = enemy.GetSpeed() - target.GetSpeed() >= 5 ? 2 : 1;

                        if (attacks == 2)
                            Console.WriteLine($"{enemy.GetName()} ataca DOS veces a {target.GetName()}.");

                        for (int i = 0; i < attacks; i++)
                        {
                            int damage = enemy.GetAttack();
                            int remainingHP = target.GetActualLife() - damage;

                            Console.WriteLine($"{enemy.GetName()} ataca a {target.GetName()} y causa {damage} de daño.");

                            if (remainingHP <= 0)
                            {
                                Console.WriteLine($"{target.GetName()} ha sido derrotado.");
                                group.Remove(target); // Remover de la copia local
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

                        int oroTrasCuracion = Healer(totalGold, allHeroes);
                        aventuraActual.SpendGold(aventuraActual.GetGold());  // reset a 0
                        aventuraActual.AddGold(oroTrasCuracion);             // oro tras curación
                        return oroTrasCuracion;
                    }
                }
            }

            // Restaurar vida y fuerza de los que no murieron
            foreach (Hero h in group)
            {
                if (h.GetActualLife() > h.GetHP())
                    h.SetActualHP(h.GetHP());

                if (originalStrength.ContainsKey(h))
                    h.SetStrength(originalStrength[h]);
            }

            // Actualizar tiendas
            bazar.Actualizar(UtilidadesTienda.FiltrarObjetosPorTipo(gameInventory.GetItems(), false, true));
            escuela.Actualizar(UtilidadesTienda.ObtenerHechizos(aventuraActual.GetInventory().GetItems()));

            return totalGold;
        }
        public static void GuardarAventura(Adventure aventura, string ruta)
        {
            using (FileStream stream = new FileStream(ruta, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, aventura);
            }
        }
        public static Adventure CargarAventura(string ruta)
        {
            using (FileStream stream = new FileStream(ruta, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return (Adventure)formatter.Deserialize(stream);
            }
        }
        static void MostrarSlots(string[] nombres)
        {
            for (int i = 0; i < nombres.Length; i++)
                Console.WriteLine($"{i + 1}. {nombres[i]}");
        }
        static string ObtenerNombreAventura(string ruta)
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
    }
}
