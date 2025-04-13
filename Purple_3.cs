using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_7
{
    public class Purple_3
    {
        public struct Participant
        {
            //конструктор
            public Participant(string name, string surname)
            {
                _name = name;
                _surname = surname;
                _marks = new double[7];
                _places = new int[7];

                counterMark = 0;
            }

            //поля
            private string _name;
            private string _surname;
            private double[] _marks;
            private int[] _places;

            private int counterMark;

            //свойства
            public string Name => _name;
            public string Surname => _surname;
            public double[] Marks
            {
                get
                {
                    if (_marks == null) return null;

                    double[] copyMarks = new double[7];
                    Array.Copy(_marks, copyMarks, 7);
                    return copyMarks;
                }
            }
            public int[] Places
            {
                get
                {
                    if (_places == null) return null;

                    int[] copyPlaces = new int[7];
                    Array.Copy(_places, copyPlaces, 7);
                    return copyPlaces;
                }
            }
            public int Score
            {
                get
                {
                    if (_places == null) return 0;
                    return _places.Sum();
                }
            }
            private int TopPlace
            {
                get
                {
                    if (_places == null) return 0;
                    return _places.Min();
                }
            }
            private double TotalMark
            {
                get
                {
                    if (_marks == null) return 0;
                    return _marks.Sum();
                }
            }

            //методы
            public void Evaluate(double result)
            {
                if (counterMark >= 7 || _marks == null) return;
                _marks[counterMark++] = result;
            }
            public static void SetPlaces(Participant[] participants)
            {
                if(participants == null) return;
                for(int judge = 0; judge < 7; judge++)
                {
                    Array.Sort(participants, (x, y) =>
                    {
                        double a = x.Marks?[judge] ?? 0;
                        double b = y.Marks?[judge] ?? 0;
                        return b.CompareTo(a);
                    });

                    for (int place = 0; place < participants.Length; place++)
                        participants[place].SetPlace(judge, place + 1);
                }
            }
            private void SetPlace(int judge, int place)
            {
                if (_places == null || judge < 0 || judge > _places.Length) return;
                _places[judge] = place;
            }

            public static void Sort(Participant[] array)
            {
                if (array == null) return;

                foreach (var participant in array)
                {
                    if (participant.Places == null) return;
                }

                Array.Sort(array, (x, y) =>
                {
                    int scoreComparison = x.Score.CompareTo(y.Score);
                    if (scoreComparison != 0) return scoreComparison;

                    int topPlaceComparison = x.TopPlace.CompareTo(y.TopPlace);
                    if (topPlaceComparison != 0) return topPlaceComparison;

                    return y.TotalMark.CompareTo(x.TotalMark);
                });
            }
            public void Print()
            {
                Console.WriteLine($"{_name} {_surname} {Score} {_places.Min()} {_marks.Sum()}");
            }
        }
        public abstract class Skating
        {
            //конструктор
            public Skating(double[] moods)
            {
                _participants = new Participant[0];

                if (moods == null || moods.Length != 7) return;

                _moods = new double[7];
                Array.Copy(moods, _moods, 7);
                ModificateMood();
            }

            //поля
            protected Participant[] _participants;
            protected double[] _moods;

            //свойства
            public Participant[] Participants => _participants;
            public double[] Moods => _moods;

            //методы
            protected abstract void ModificateMood();
            public void Evaluate(double[] marks)
            {
                if (_participants == null || marks == null) return;

                foreach (var participant in _participants)
                {
                    if (participant.Marks.All(mark => mark == 0))
                    {
                        for (int i = 0; i < _moods.Length; i++)
                        {
                            participant.Evaluate(marks[i] * Moods[i]);
                        }
                        break;
                    }
                }
            }
            public void Add(Participant participant)
            {
                if (_participants == null)
                    _participants = new Participant[0];

                Array.Resize(ref _participants, _participants.Length + 1);
                _participants[_participants.Length - 1] = participant;
            }
            public void Add(Participant[] participants)
            {
                if (participants == null) return;

                foreach (var participant in participants)
                {
                    Add(participant);
                }
            }
        }
        public class FigureSkating : Skating
        {
            //конструктор
            public FigureSkating(double[] moods) : base(moods) { }

            //метод
            protected override void ModificateMood()
            {
                if (_moods == null) return;

                _moods = _moods
                    .Select((mood, judge) => mood + (judge + 1) / 10.0)
                    .ToArray();
            }
        }
        public class IceSkating : Skating
        {
            //конструктор
            public IceSkating(double[] moods) : base(moods) { }

            //метод
            protected override void ModificateMood()
            {
                if (_moods == null) return;

                _moods = _moods
                    .Select((mood, judge) => mood * (1 + (judge + 1) / 100.0))
                    .ToArray();
            }
        }

    }

}
        