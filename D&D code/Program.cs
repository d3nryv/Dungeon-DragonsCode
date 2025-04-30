using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace D_D_code
{
    class Hero
    {
        // Atributes
        string heroName;
        string heroTitle;
        string heroRace;
        string heroClass;
        string heroLore;
        DateTime heroReclutationTime;
        int heroStrength;
        int heroConstitution;
        int heroHP;
        int heroInteligence;
        int heroSpeed;
        int heroActualHP;
        int heroLvl;
        int heroXP;

        // Constructor
        public Hero(string intName, string intTitle, string intRace, string intClass, string intLore, int intStrength, int intConstitution, int intHP, int intIntelligence, int intSpeed)
        {
            heroName = intName;
            heroTitle = intTitle;
            heroRace = intRace;
            heroClass = intClass;
            heroLore = intLore;
            heroStrength = intStrength;
            heroConstitution = intConstitution;
            heroHP = intHP;
            heroInteligence = intIntelligence;
            heroSpeed = intSpeed;

            // Modificadores por raza
            if (heroRace == "Humano")
            {
                heroHP = heroHP + 1;
                heroInteligence = heroInteligence + 1;
            }
            if (heroRace == "Elfo")
            {
                heroInteligence = heroInteligence + 2;
                heroSpeed = heroSpeed + 1;
            }
            if (heroRace == "Orco")
            {
                heroStrength = heroStrength + 2;
                heroConstitution = heroConstitution + 1;
            }
            if (heroRace == "Enano")
            {
                heroConstitution = heroConstitution + 2;
                heroHP = heroHP + 1;
            }

            // Modificadores por clase
            if (heroClass == "Guerrero")
            {
                heroStrength = heroStrength + 2;
                heroConstitution = heroConstitution + 1;
            }
            if (heroClass == "Mago")
            {
                heroInteligence = heroInteligence + 2;
                heroHP = heroHP + 1;
            }
            if (heroClass == "Sanador")
            {
                heroConstitution = heroConstitution + 2;
                heroInteligence = heroInteligence + 1;
            }
            if (heroClass == "Ladrón")
            {
                heroSpeed = heroSpeed + 2;
                heroStrength = heroStrength + 1;
            }

            heroActualHP = heroHP;
            heroLvl = 1;
            heroXP = 0;
            heroReclutationTime = DateTime.Now;
        }

        // Setters
        public void SetName(string newName) { heroName = newName; }
        public void SetTitle(string newTitle) { heroTitle = newTitle; }
        public void SetRace(string newRace) { heroRace = newRace; }
        public void SetClass(string newClass) { heroClass = newClass; }
        public void SetLore(string newLore) { heroLore = newLore; }
        public void SetStrength(int newStrength) { heroStrength = newStrength; }
        public void SetConstitution(int newConstitution) { heroConstitution = newConstitution; }
        public void SetHP(int newHP) { heroHP = newHP; }
        public void SetInteligence(int newInteligence) { heroInteligence = newInteligence; }
        public void SetSpeed(int newSpeed) { heroSpeed = newSpeed; }
        public void SetActualHP(int newActualHP) { heroActualHP = newActualHP; }
        public void SetLvl(int newLvl) { heroLvl = newLvl; }
        public void SetXP(int newXP) { heroXP = newXP; }

        // Getters
        public string GetName() { return heroName; }
        public string GetTitle() { return heroTitle; }
        public string GetRace() { return heroRace; }
        public string GetClass() { return heroClass; }
        public string GetLore() { return heroLore; }
        public DateTime GetReclutationTime() { return heroReclutationTime; }
        public int GetStrength() { return heroStrength; }
        public int GetConstitution() { return heroConstitution; }
        public int GetHP() { return heroHP; }
        public int GetInteligence() { return heroInteligence; }
        public int GetVelocity() { return heroSpeed; }
        public int GetActualLife() { return heroActualHP; }
        public int GetLvl() { return heroLvl; }
        public int GetXP() { return heroXP; }

        //Modificators
        public void AddXP(int gainedXP)
        {
            heroXP = heroXP + gainedXP;

            if (heroXP >= 100)
            {
                heroXP = heroXP - 100;
                heroLvl = heroLvl + 1;

                int oldHP = heroHP;
                int oldStrength = heroStrength;
                int oldSpeed = heroSpeed;

                heroHP = heroHP + 1;
                heroStrength = heroStrength + 2;
                heroSpeed = heroSpeed + 1;
                heroActualHP = heroHP;

                Console.WriteLine("¡" + heroName + " ha subido al nivel " + heroLvl + "!");
                Console.WriteLine("HP -> " + oldHP + " + 1");
                Console.WriteLine("Strength -> " + oldStrength + " + 2");
                Console.WriteLine("Speed -> " + oldSpeed + " + 1");
                Console.WriteLine("Constitution -> " + heroConstitution);
                Console.WriteLine("Intelligence -> " + heroInteligence);
                Console.WriteLine("");
            }
        }
    }
    class Enemy
    {
        // Atributos
        string nombre;
        int resistencia;
        int ataque;
        int vida;
        int velocidad;
        int dinero;
        int experiencia;
        bool resistenciaFisica;
        bool inmunidadMagica;

        // Constructor
        public Enemy(string newNombre, int newResistencia, int newAtaque, int newVida, int newVelocidad, int newDinero, int newExperiencia, bool newResistenciaFisica, bool newInmunidadMagica)
        {
            nombre = newNombre;
            resistencia = newResistencia;
            ataque = newAtaque;
            vida = newVida;
            velocidad = newVelocidad;
            dinero = newDinero;
            experiencia = newExperiencia;
            resistenciaFisica = newResistenciaFisica;
            inmunidadMagica = newInmunidadMagica;
        }

        // Setters
        public void SetNombre(string newNombre)
        {
            nombre = newNombre;
        }
        public void SetResistencia(int newResistencia)
        {
            resistencia = newResistencia;
        }
        public void SetAtaque(int newAtaque)
        {
            ataque = newAtaque;
        }
        public void SetVida(int newVida)
        {
            vida = newVida;
        }
        public void SetVelocidad(int newVelocidad)
        {
            velocidad = newVelocidad;
        }
        public void SetDinero(int newDinero)
        {
            dinero = newDinero;
        }
        public void SetExperiencia(int newExperiencia)
        {
            experiencia = newExperiencia;
        }
        public void SetResistenciaFisica(bool newResistenciaFisica)
        {
            resistenciaFisica = newResistenciaFisica;
        }
        public void SetInmunidadMagica(bool newInmunidadMagica)
        {
            inmunidadMagica = newInmunidadMagica;
        }

        // Getters
        public string GetNombre()
        {
            return nombre;
        }
        public int GetResistencia()
        {
            return resistencia;
        }
        public int GetAtaque()
        {
            return ataque;
        }
        public int GetVida()
        {
            return vida;
        }
        public int GetVelocidad()
        {
            return velocidad;
        }
        public int GetDinero()
        {
            return dinero;
        }
        public int GetExperiencia()
        {
            return experiencia;
        }
        public bool GetResistenciaFisica()
        {
            return resistenciaFisica;
        }
        public bool GetInmunidadMagica()
        {
            return inmunidadMagica;
        }
    }
    class HeroAttack
    {
        public Hero hero;
        public Enemy enemy;

        public HeroAttack(Hero h, Enemy e)
        {
            hero = h;
            enemy = e;
        }
    }
    class CombatAction
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

    internal class Program
    {
        static void Main(string[] args)
        {
            List<Hero> todosLosHeroes = new List<Hero>();
            todosLosHeroes.Add(new Hero("Thalendir", "El Escudo", "Humano", "Guerrero", "Capitán", 5, 3, 5, 2, 4));
            todosLosHeroes.Add(new Hero("Elandra", "La Dama", "Elfa", "Maga", "Guardiana", 4, 2, 4, 5, 6));
            todosLosHeroes.Add(new Hero("Gorak", "Puño de Hierro", "Orco", "Sanador", "Chamán", 3, 5, 6, 2, 3));

            List<Hero> heroesVivos = new List<Hero>();

            int counter = 0;
            while (counter < todosLosHeroes.Count)
            {
                heroesVivos.Add(todosLosHeroes[counter]);
                counter = counter + 1;
            }

            int oroTotal = 50;
            Random random = new Random();
            bool jugando = true;

            Console.WriteLine("¡Bienvenido aventurero!");
            Console.WriteLine("");

            while (jugando == true)
            {
                Console.WriteLine("¿Qué deseas hacer?");
                Console.WriteLine("1. Ver Héroes");
                Console.WriteLine("2. Ir a Mazmorra");
                Console.WriteLine("3. Curar en Taberna");
                Console.WriteLine("4. Salir");
                Console.Write("Selección: ");
                string opcion = Console.ReadLine();
                Console.WriteLine("");

                switch (opcion)
                {
                    case "1":
                        {
                            MostrarHeroes(heroesVivos);
                            break;
                        }

                    case "2":
                        {
                            Console.WriteLine("Selecciona tus héroes para la expedición (máximo 4).");
                            Console.WriteLine("");

                            List<Hero> grupoSeleccionado = new List<Hero>();

                            int heroCounter = 0;
                            while (heroCounter < heroesVivos.Count)
                            {
                                Hero h = heroesVivos[heroCounter];
                                Console.WriteLine((heroCounter + 1) + ". " + h.GetName());
                                heroCounter = heroCounter + 1;
                            }

                            Console.WriteLine("");

                            int countSeleccionados = 0;
                            while (countSeleccionados < 4 && grupoSeleccionado.Count < heroesVivos.Count)
                            {
                                Console.Write("Introduce el número del héroe que quieres seleccionar: ");
                                string seleccion = Console.ReadLine();

                                int seleccionNumero;
                                bool esNumero = int.TryParse(seleccion, out seleccionNumero);

                                if (esNumero == true && seleccionNumero >= 1 && seleccionNumero <= heroesVivos.Count)
                                {
                                    Hero heroeElegido = heroesVivos[seleccionNumero - 1];

                                    if (grupoSeleccionado.Contains(heroeElegido) == false)
                                    {
                                        grupoSeleccionado.Add(heroeElegido);
                                        Console.WriteLine(heroeElegido.GetName() + " ha sido añadido al grupo.");
                                        Console.WriteLine("");
                                        countSeleccionados = countSeleccionados + 1;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Ese héroe ya está en tu grupo.");
                                        Console.WriteLine("");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Selección inválida.");
                                    Console.WriteLine("");
                                }
                            }

                            Console.WriteLine("¡Expedición lista! Pulsa ENTER para comenzar la aventura.");
                            Console.ReadLine();
                            Console.WriteLine("");

                            oroTotal = Dungeon(grupoSeleccionado, oroTotal, random, heroesVivos);

                            if (oroTotal <= 0)
                            {
                                jugando = false;
                            }

                            break;
                        }

                    case "3":
                        {
                            oroTotal = Healer(oroTotal, heroesVivos);
                            if (oroTotal <= 0)
                            {
                                jugando = false;
                            }
                            break;
                        }

                    case "4":
                        {
                            Console.WriteLine("¡Gracias por jugar aventurero!");
                            jugando = false;
                            break;
                        }

                    default:
                        {
                            Console.WriteLine("Opción inválida.");
                            Console.WriteLine("");
                            break;
                        }
                }
            }
        }

        static void MostrarHeroes(List<Hero> heroesVivos)
        {
            Console.WriteLine("Héroes Reclutados:");
            Console.WriteLine("");

            int heroCounter = 0;
            while (heroCounter < heroesVivos.Count)
            {
                Hero h = heroesVivos[heroCounter];
                Console.WriteLine(h.GetName());
                Console.WriteLine("|");
                Console.WriteLine("Raza: " + h.GetRace());
                Console.WriteLine("Clase: " + h.GetClass());
                Console.WriteLine("Vida: " + h.GetActualLife() + "/" + h.GetHP());
                Console.WriteLine("Fuerza: " + h.GetStrength());
                Console.WriteLine("Constitución: " + h.GetConstitution());
                Console.WriteLine("Inteligencia: " + h.GetInteligence());
                Console.WriteLine("Velocidad: " + h.GetVelocity());
                Console.WriteLine("Nivel: " + h.GetLvl());
                Console.WriteLine("Experiencia: " + h.GetXP() + "/100");
                Console.WriteLine("");
                heroCounter = heroCounter + 1;
            }

            Console.WriteLine("");
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
        static int Dungeon(List<Hero> grupo, int oroTotal, Random random, List<Hero> heroesVivos)
        {
            List<Enemy> monstruos = new List<Enemy>();
            monstruos.Add(new Enemy("Gólem", 3, 2, 5, 3, 5, 15, true, true));
            monstruos.Add(new Enemy("Esqueleto", 2, 1, 3, 4, 3, 10, false, false));

            int oroInicial = oroTotal;
            bool combateActivo = true;

            List<CombatAction> acciones = new List<CombatAction>();
            int counterAccion = 0;
            while (counterAccion < grupo.Count)
            {
                acciones.Add(new CombatAction(grupo[counterAccion]));
                counterAccion++;
            }

            while (combateActivo)
            {
                List<HeroAttack> ataquesPendientes = new List<HeroAttack>();
                int counterHeroe = 0;
                while (counterHeroe < grupo.Count)
                {
                    if (monstruos.Count > 0)
                    {
                        Hero h = grupo[counterHeroe];
                        Console.WriteLine("Turno de " + h.GetName());
                        Console.WriteLine("Elige monstruo para atacar:");

                        int counterMonstruo = 0;
                        while (counterMonstruo < monstruos.Count)
                        {
                            Console.WriteLine((counterMonstruo + 1) + ". " + monstruos[counterMonstruo].GetNombre() + " (Vida: " + monstruos[counterMonstruo].GetVida() + ")");
                            counterMonstruo++;
                        }

                        Console.Write("Selecciona: ");
                        string eleccion = Console.ReadLine();

                        int numeroSeleccionado;
                        bool esNumero = int.TryParse(eleccion, out numeroSeleccionado);

                        if (esNumero == true && numeroSeleccionado >= 1 && numeroSeleccionado <= monstruos.Count)
                        {
                            Enemy objetivo = monstruos[numeroSeleccionado - 1];
                            ataquesPendientes.Add(new HeroAttack(h, objetivo));

                            int contadorAccion = 0;
                            while (contadorAccion < acciones.Count)
                            {
                                if (acciones[contadorAccion].hero == h)
                                {
                                    acciones[contadorAccion].attacked = true;
                                }
                                contadorAccion++;
                            }
                        }

                        Console.WriteLine("");
                    }
                    counterHeroe++;
                }

                // Ordenar ataques por velocidad
                int i = 0;
                while (i < ataquesPendientes.Count - 1)
                {
                    int j = i + 1;
                    while (j < ataquesPendientes.Count)
                    {
                        if (ataquesPendientes[j].hero.GetVelocity() > ataquesPendientes[i].hero.GetVelocity())
                        {
                            HeroAttack temp = ataquesPendientes[i];
                            ataquesPendientes[i] = ataquesPendientes[j];
                            ataquesPendientes[j] = temp;
                        }
                        j++;
                    }
                    i++;
                }

                int counterAtaque = 0;
                while (counterAtaque < ataquesPendientes.Count)
                {
                    Hero h = ataquesPendientes[counterAtaque].hero;
                    Enemy enemigo = ataquesPendientes[counterAtaque].enemy;

                    // Reemplazo de Contains
                    bool enemigoSigueVivo = false;
                    int buscar = 0;
                    while (buscar < monstruos.Count)
                    {
                        if (monstruos[buscar] == enemigo)
                        {
                            enemigoSigueVivo = true;
                        }
                        buscar++;
                    }

                    if (enemigoSigueVivo == true)
                    {
                        Console.WriteLine(h.GetName() + " ataca a " + enemigo.GetNombre());
                        int dado = random.Next(1, 11);
                        Console.WriteLine("Tirada: (" + dado + ")");

                        int fuerza = h.GetStrength();
                        if (dado == 10)
                        {
                            Console.WriteLine("¡Golpe crítico!");
                            fuerza = (int)(fuerza * 1.25);
                        }

                        if (dado >= 6)
                        {
                            if (fuerza >= enemigo.GetVida())
                            {
                                Console.WriteLine("¡Derrotaste al " + enemigo.GetNombre() + "!");
                                oroTotal = oroTotal + enemigo.GetDinero();

                                int eliminar = 0;
                                while (eliminar < monstruos.Count)
                                {
                                    if (monstruos[eliminar] == enemigo)
                                    {
                                        monstruos.RemoveAt(eliminar);
                                        break;
                                    }
                                    eliminar++;
                                }

                                int contadorXP = 0;
                                while (contadorXP < acciones.Count)
                                {
                                    if (acciones[contadorXP].hero == h)
                                    {
                                        acciones[contadorXP].killed = true;
                                        acciones[contadorXP].xpExtra = acciones[contadorXP].xpExtra + enemigo.GetExperiencia();
                                    }
                                    contadorXP++;
                                }
                            }
                            else
                            {
                                Console.WriteLine("No lo derrotaste.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Fallaste el ataque.");
                            if (fuerza >= enemigo.GetVida())
                            {
                                int contadorFallo = 0;
                                while (contadorFallo < acciones.Count)
                                {
                                    if (acciones[contadorFallo].hero == h)
                                    {
                                        acciones[contadorFallo].couldHaveKilledButMissed = true;
                                    }
                                    contadorFallo++;
                                }
                            }
                        }

                        Console.WriteLine("");
                    }

                    counterAtaque++;
                }

                // Restante del combate o enemigos vivos
                if (monstruos.Count == 0)
                {
                    Console.WriteLine("¡Has limpiado la mazmorra!");
                    combateActivo = false;

                    int oroGanado = oroTotal - oroInicial;
                    if (oroGanado < 0)
                    {
                        oroGanado = 0;
                    }

                    Console.WriteLine();
                    Console.WriteLine("=== Resumen del combate ===");
                    Console.WriteLine("Oro ganado: " + oroGanado);
                    Console.WriteLine("Oro total: " + oroTotal);
                    Console.WriteLine();

                    int resumenCounter = 0;
                    while (resumenCounter < acciones.Count)
                    {
                        CombatAction acc = acciones[resumenCounter];
                        int xpGanada = acc.xpExtra;

                        if (acc.killed)
                        {
                            // XP ya sumada
                        }
                        else if (acc.couldHaveKilledButMissed)
                        {
                            xpGanada = xpGanada + 10;
                        }
                        else if (acc.attacked)
                        {
                            xpGanada = xpGanada + 5;
                        }

                        if (xpGanada == 0)
                        {
                            Console.WriteLine(acc.hero.GetName() + " no ha participado y no gana XP.");
                        }
                        else
                        {
                            Console.WriteLine(acc.hero.GetName() + " gana " + xpGanada + " XP.");
                            acc.hero.AddXP(xpGanada);
                        }

                        resumenCounter++;
                    }

                    Console.WriteLine("===========================\n");
                }
                else
                {
                    // Turno de los monstruos
                    int contadorEnemigo = 0;
                    while (contadorEnemigo < monstruos.Count)
                    {
                        Enemy enemigo = monstruos[contadorEnemigo];
                        if (grupo.Count > 0)
                        {
                            int objetivoIndex = random.Next(0, grupo.Count);
                            Hero objetivo = grupo[objetivoIndex];

                            Console.WriteLine(enemigo.GetNombre() + " ataca a " + objetivo.GetName());
                            int dano = enemigo.GetAtaque();
                            int nuevaVida = objetivo.GetActualLife() - dano;
                            if (nuevaVida < 0)
                            {
                                nuevaVida = 0;
                            }

                            objetivo.SetActualHP(nuevaVida);
                            Console.WriteLine("¡" + objetivo.GetName() + " ha recibido " + dano + " puntos de daño!");
                            Console.WriteLine("Vida restante: " + nuevaVida + "/" + objetivo.GetHP());

                            if (nuevaVida == 0)
                            {
                                Console.WriteLine(objetivo.GetName() + " ha sido derrotado.");
                                grupo.Remove(objetivo);
                            }

                            Console.WriteLine("");
                        }
                        contadorEnemigo++;
                    }

                    if (grupo.Count == 0)
                    {
                        Console.WriteLine("¡Todos tus héroes han caído en combate!");
                        combateActivo = false;
                        oroTotal = Healer(oroTotal, heroesVivos);
                    }
                }
            }

            return oroTotal;
        }
    }
}
