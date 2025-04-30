using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public Hero(string intName, string intTitle, string intRace, string intClass, string intLore, int intStrength, int intConstitution, int intHP, int intrInteligence, int intSpeed)
        {
            heroName = intName;
            heroTitle = intTitle;
            heroRace = intRace;
            heroClass = intClass;
            heroLore = intLore;
            heroStrength = intStrength;
            heroConstitution = intConstitution;
            heroHP = intHP;
            heroInteligence = intrInteligence;
            heroSpeed = intSpeed;

            heroActualHP = heroHP;
            heroLvl = 1;
            heroXP = 0;
            heroReclutationTime = DateTime.Now;
        }

        // Setters
        public void SetName(string newName)
        {
            heroName = newName;
        }
        public void SetTitle(string newTitle)
        {
            heroTitle = newTitle;
        }
        public void SetRace(string newRace)
        {
            heroRace = newRace;
        }
        public void SetClass(string newClass)
        {
            heroClass = newClass;
        }
        public void SetLore(string newLore)
        {
            heroLore = newLore;
        }
        public void SetStrength(int newStrength)
        {
            heroStrength = newStrength;
        }
        public void SetConstitution(int newConstitution)
        {
            heroConstitution = newConstitution;
        }
        public void SetHP(int newHP)
        {
            heroHP = newHP;
        }
        public void SetInteligence(int newInteligence)
        {
            heroInteligence = newInteligence;
        }
        public void SetSpeed(int newSpeed)
        {
            heroSpeed = newSpeed;
        }
        public void SetActualHP(int newActualHP)
        {
            heroActualHP = newActualHP;
        }
        public void SetLvl(int newLvl)
        {
            heroLvl = newLvl;
        }
        public void SetXP(int newXP)
        {
            heroXP = newXP;
        }

        // Getters
        public string GetName()
        {
            return heroName;
        }
        public string GetTitle()
        {
            return heroTitle;
        }
        public string GetRace()
        {
            return heroRace;
        }
        public string GetClass()
        {
            return heroClass;
        }
        public string GetLore()
        {
            return heroLore;
        }
        public DateTime GetReclutationTime()
        {
            return heroReclutationTime;
        }
        public int GetStrength()
        {
            return heroStrength;
        }
        public int GetConstitution()
        {
            return heroConstitution;
        }
        public int GetHP()
        {
            return heroHP;
        }
        public int GetInteligence()
        {
            return heroInteligence;
        }
        public int GetVelocity()
        {
            return heroSpeed;
        }
        public int GetActualLife()
        {
            return heroActualHP;
        }
        public int GetLvl()
        {
            return heroLvl;
        }
        public int GetXP()
        {
            return heroXP;
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

                if (opcion == "1")
                {
                    Console.WriteLine("Héroes Reclutados:");
                    Console.WriteLine("");

                    int heroCounter = 0;
                    while (heroCounter < heroesVivos.Count)
                    {
                        Hero h = heroesVivos[heroCounter];
                        Console.WriteLine(h.GetName());
                        Console.WriteLine("Vida: " + h.GetActualLife() + "/" + h.GetHP());
                        Console.WriteLine("Ataque: " + h.GetStrength());
                        Console.WriteLine("Nivel: " + h.GetLvl());
                        Console.WriteLine("Experiencia: " + h.GetXP());
                        Console.WriteLine("");
                        heroCounter = heroCounter + 1;
                    }

                    Console.WriteLine("");
                }
                else
                {
                    if (opcion == "2")
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
                    }
                    else
                    {
                        if (opcion == "3")
                        {
                            if (Healer(oroTotal, heroesVivos) == false)
                            {
                                jugando = false;
                            }
                        }
                        else
                        {
                            if (opcion == "4")
                            {
                                Console.WriteLine("¡Gracias por jugar aventurero!");
                                jugando = false;
                            }
                            else
                            {
                                Console.WriteLine("Opción inválida.");
                                Console.WriteLine("");
                            }
                        }
                    }
                }
            }
        }

        static int Dungeon(List<Hero> grupo, int oroTotal, Random random, List<Hero> heroesVivos)
        {
            List<Enemy> monstruos = new List<Enemy>();
            monstruos.Add(new Enemy("Gólem", 3, 2, 5, 3, 5, 30, true, true));
            monstruos.Add(new Enemy("Esqueleto", 2, 1, 3, 4, 3, 20, false, false));

            bool combateActivo = true;

            while (combateActivo)
            {
                List<(Hero, Enemy)> accionesPendientes = new List<(Hero, Enemy)>();

                int i = 0;
                while (i < grupo.Count)
                {
                    if (monstruos.Count > 0)
                    {
                        Hero h = grupo[i];

                        Console.WriteLine("Turno de " + h.GetName());
                        Console.WriteLine("Elige monstruo para atacar:");

                        int j = 0;
                        while (j < monstruos.Count)
                        {
                            Console.WriteLine((j + 1) + ". " + monstruos[j].GetNombre() + " (Vida: " + monstruos[j].GetVida() + ")");
                            j = j + 1;
                        }

                        Console.Write("Selecciona: ");
                        string eleccion = Console.ReadLine();

                        int n;
                        if (int.TryParse(eleccion, out n) && n >= 1 && n <= monstruos.Count)
                        {
                            Enemy objetivo = monstruos[n - 1];
                            accionesPendientes.Add((h, objetivo));
                        }

                        Console.WriteLine("");
                    }
                    i = i + 1;
                }

                // Ordenar por velocidad descendente
                int a = 0;
                while (a < accionesPendientes.Count - 1)
                {
                    int b = a + 1;
                    while (b < accionesPendientes.Count)
                    {
                        if (accionesPendientes[b].Item1.GetVelocity() > accionesPendientes[a].Item1.GetVelocity())
                        {
                            var temp = accionesPendientes[a];
                            accionesPendientes[a] = accionesPendientes[b];
                            accionesPendientes[b] = temp;
                        }
                        b = b + 1;
                    }
                    a = a + 1;
                }

                int at = 0;
                while (at < accionesPendientes.Count)
                {
                    Hero h = accionesPendientes[at].Item1;
                    Enemy objetivo = accionesPendientes[at].Item2;

                    if (monstruos.Contains(objetivo))
                    {
                        Console.WriteLine(h.GetName() + " ataca a " + objetivo.GetNombre());

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
                            if (fuerza >= objetivo.GetVida())
                            {
                                Console.WriteLine("¡Derrotaste al " + objetivo.GetNombre() + "!");
                                oroTotal = oroTotal + objetivo.GetDinero();
                                monstruos.Remove(objetivo);
                            }
                            else
                            {
                                Console.WriteLine("No lo derrotaste.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Fallaste el ataque.");
                        }

                        Console.WriteLine("");
                    }

                    at = at + 1;
                }

                if (monstruos.Count == 0)
                {
                    Console.WriteLine("¡Has limpiado la mazmorra!");
                    combateActivo = false;
                }
                else
                {
                    // Turno de los monstruos
                    int m = 0;
                    while (m < monstruos.Count)
                    {
                        Enemy enemigo = monstruos[m];

                        // Ataca a un héroe aleatorio del grupo
                        if (grupo.Count > 0)
                        {
                            int indiceObjetivo = random.Next(0, grupo.Count);
                            Hero objetivo = grupo[indiceObjetivo];

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

                        m = m + 1;
                    }

                    if (grupo.Count == 0)
                    {
                        Console.WriteLine("¡Todos tus héroes han caído en combate!");
                        combateActivo = false;
                        Healer(oroTotal, heroesVivos);
                    }
                }
            }
            return oroTotal;
        }
        static bool Healer(int oroTotal, List<Hero> heroesVivos)
        {
            if (heroesVivos.Count == 0)
            {
                Console.WriteLine("No tienes héroes vivos para curar.");
                Console.WriteLine("");
            }
            else
            {
                int totalCoste = 0;

                int contador = 0;
                while (contador < heroesVivos.Count)
                {
                    Hero h = heroesVivos[contador];
                    int vidaMax = h.GetHP();
                    int vidaActual = h.GetActualLife();

                    if (vidaActual < vidaMax)
                    {
                        int porcentajeFaltante = (vidaMax - vidaActual) * 100 / vidaMax;
                        int coste = (porcentajeFaltante / 20) * 2;
                        if (coste == 0)
                        {
                            coste = 2;
                        }
                        totalCoste = totalCoste + coste;
                    }
                    contador = contador + 1;
                }

                Console.WriteLine("Coste total de curación: " + totalCoste + " de oro.");
                Console.WriteLine("Te quedan: " + oroTotal + " de oro.");
                Console.WriteLine("");

                if (oroTotal >= totalCoste && totalCoste > 0)
                {
                    oroTotal = oroTotal - totalCoste;

                    int c = 0;
                    while (c < heroesVivos.Count)
                    {
                        Hero h = heroesVivos[c];
                        if (h.GetActualLife() < h.GetHP())
                        {
                            h.SetActualHP(h.GetHP());
                        }
                        c = c + 1;
                    }

                    Console.WriteLine("¡Héroes curados correctamente!");
                }
                else
                {
                    if (totalCoste == 0)
                    {
                        Console.WriteLine("Todos tus héroes ya están con la vida al máximo.");
                    }
                    else
                    {
                        Console.WriteLine("No tienes suficiente oro para curar.");
                        Console.WriteLine("GAME OVER.");
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
