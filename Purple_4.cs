using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_7
{
    public class Purple_4
    {
        public class Sportsman
        {
            //конструктор
            public Sportsman(string name, string surname)
            {
                _name = name;
                _surname = surname;
                _time = 0;
            }
            //поля
            private string _name;
            private string _surname;
            private double _time;

            //свойства
            public string Name => _name;
            public string Surname => _surname;
            public double Time => _time;

            //методы
            public void Run(double time)
            {
                if (_time == 0) _time = time;
            }
            public void Print()
            {
                Console.WriteLine($"{_name} {_surname} {_time}");
            }
            public static void Sort(Sportsman[] array)
            {
                Group sortArray = new Group(""); //для инициализации массива
                sortArray.Add(array);
                sortArray.Sort();

                Array.Copy(sortArray.Sportsmen, array, array.Length);
            }
        }
        public class SkiMan : Sportsman
        {
            //конструкторы
            public SkiMan(string name, string surname) : base(name, surname) { }
            public SkiMan(string name, string surname, double time) : base(name, surname)
            {
                Run(time);
            }
        }
        public class SkiWoman : Sportsman
        {
            //конструкторы
            public SkiWoman(string name, string surname) : base(name, surname) { }
            public SkiWoman(string name, string surname, double time) : base(name, surname)
            {
                Run(time);
            }
        }
        public class Group
        {
            //конструкторы
            public Group(string name)
            {
                _name = name;
                _sportsmen = new Sportsman[0];
            }
            public Group(Group group)
            {
                _name = group.Name;
                if (group.Sportsmen != null)
                    _sportsmen = group.Sportsmen;
                else
                    _sportsmen = null;
            }

            //поля
            private string _name;
            private Sportsman[] _sportsmen;

            //свойства
            public string Name => _name;
            public Sportsman[] Sportsmen
            {
                get
                {
                    if (_sportsmen == null) return null;

                    Sportsman[] copySportsmen = new Sportsman[_sportsmen.Length];
                    Array.Copy(_sportsmen, copySportsmen, _sportsmen.Length);
                    return copySportsmen;
                }
            }

            //методы
            public void Add(Sportsman sportsman)
            {
                if (_sportsmen == null) return;

                Array.Resize(ref _sportsmen, _sportsmen.Length + 1);
                _sportsmen[_sportsmen.Length - 1] = sportsman;
            }
            public void Add(Sportsman[] sportsmen)
            {
                if (sportsmen == null || _sportsmen == null) return;

                foreach (var sportsman in sportsmen)
                {
                    Add(sportsman);
                }
            }
            public void Add(Group group)
            {
                if (group.Sportsmen == null || _sportsmen == null) return;

                int before = _sportsmen.Length;
                Array.Resize(ref _sportsmen, before + group.Sportsmen.Length);
                for (int i = 0; i < group.Sportsmen.Length; i++)
                {
                    _sportsmen[before + i] = group.Sportsmen[i];
                }
            }
            public void Sort()
            {
                if (_sportsmen == null) return;

                for (int i = 1, j = 2; i < _sportsmen.Length;)
                {
                    if (i == 0 || _sportsmen[i - 1].Time <= _sportsmen[i].Time)
                    {
                        i = j;
                        j++;
                    }
                    else
                    {
                        (_sportsmen[i - 1], _sportsmen[i]) = (_sportsmen[i], _sportsmen[i - 1]);
                        i--;
                    }
                }
            }
            public static Group Merge(Group group1, Group group2)
            {
                Group mergeredGroup = new Group("Финалисты");

                if (group1.Sportsmen == null || group2.Sportsmen == null) return mergeredGroup;

                int i = 0, j = 0;

                while (i < group1.Sportsmen.Length && j < group2.Sportsmen.Length)
                {
                    if (group1.Sportsmen[i].Time <= group2.Sportsmen[j].Time)
                    {
                        mergeredGroup.Add(group1.Sportsmen[i++]);
                    }
                    else
                    {
                        mergeredGroup.Add(group2.Sportsmen[j++]);
                    }
                }
                while (i < group1.Sportsmen.Length)
                {
                    mergeredGroup.Add(group1.Sportsmen[i++]);
                }
                while (j < group2.Sportsmen.Length)
                {
                    mergeredGroup.Add(group2.Sportsmen[j++]);
                }

                return mergeredGroup;
            }
            public void Split(out Sportsman[] men, out Sportsman[] women)
            {
                men = new SkiMan[0];
                women = new SkiWoman[0];

                if (_sportsmen == null) return;

                foreach (var sportsman in _sportsmen)
                {
                    if (sportsman is SkiMan skiMan)
                    {
                        Array.Resize(ref men, men.Length + 1);
                        men[men.Length - 1] = skiMan;
                    }

                    if (sportsman is SkiWoman skiWoman)
                    {
                        Array.Resize(ref women, women.Length + 1);
                        women[women.Length - 1] = skiWoman;
                    }
                }
            }
            public void Shuffle()
            {
                Sportsman[] men, women;
                Split(out men, out women);

                Sportsman.Sort(men);
                Sportsman.Sort(women);

                int i = 0, j = 0, k = 0;
                while (j < men.Length && k < women.Length)
                {
                    if (men[0].Time <= women[0].Time)
                    {
                        _sportsmen[i++] = men[j++];
                        _sportsmen[i++] = women[k++];
                    }
                    else
                    {
                        _sportsmen[i++] = women[k++];
                        _sportsmen[i++] = men[j++];
                    }
                }
                while (j < men.Length)
                    _sportsmen[i++] = men[j++];
                while (k < women.Length)
                    _sportsmen[i++] = women[k++];
            }
            public void Print()
            {
                Console.WriteLine(_name);
                foreach (var sportsman in _sportsmen)
                {
                    sportsman.Print();
                }
            }
        }
    }
}
