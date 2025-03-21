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

            //методы
            public void Evaluate(double result)
            {
                if (counterMark >= 7 || _marks == null) return;
                _marks[counterMark++] = result;
            }
            public static void SetPlaces(Participant[] participants)
            {
                if(participants == null) return;

                for(int judge = 0; judge < 7; judge++) //проходимся по оценкам у каждого судьи
                {
                    double[] copyMarks = new double[participants.Length]; //массив оценок всех участников у конкретного судьи
                    for(int i = 0; i < participants.Length; i++)
                    {
                        if (participants[i]._marks == null) return;
                        copyMarks[i] = participants[i]._marks[judge];
                    }
                    
                    Participant[] copyParticipants = new Participant[participants.Length];
                    Array.Copy(participants, copyParticipants, participants.Length);

                    for (int i = 1, j = 2; i < participants.Length; )
                    {
                        if(i == 0 || copyMarks[i-1] >= copyMarks[i])
                        {
                            i = j;
                            j++;
                        }
                        else
                        {
                            (copyMarks[i - 1], copyMarks[i]) = (copyMarks[i], copyMarks[i - 1]);
                            (copyParticipants[i - 1], copyParticipants[i]) = (copyParticipants[i], copyParticipants[i - 1]);
                            i--;
                        }
                    } //параллельно сортируем копию массива оценок у судьи и копию массива участников

                    int[] places = new int[participants.Length]; //массив мест у конкретного судьи
                    places[0] = 1;
                    for(int i = 1; i < places.Length; i++)
                    {
                        if (copyMarks[i] == copyMarks[i - 1])
                        {
                            places[i] = places[i - 1];
                        }
                        else
                        {
                            places[i] = places[i - 1] + 1;
                        }
                    }

                    for (int i = 0; i < participants.Length; i++)
                    {
                        if (copyParticipants[i]._places == null) return;
                        copyParticipants[i]._places[judge] = places[i];
                    }
                    
                }

                int[] lastPlaces = new int[participants.Length]; //сортируем по возрастанию мест у последнего судьи
                for(int i = 0; i < lastPlaces.Length; i++)
                {
                    lastPlaces[i] = participants[i]._places[6];
                }
                for (int i = 1, j = 2; i < participants.Length;)
                {
                    if (i == 0 || lastPlaces[i-1] <= lastPlaces[i])
                    {
                        i = j;
                        j++;
                    }
                    else
                    {
                        (lastPlaces[i - 1], lastPlaces[i]) = (lastPlaces[i], lastPlaces[i - 1]);
                        (participants[i - 1], participants[i]) = (participants[i], participants[i - 1]);
                        i--;
                    }
                }
            }
            public static void Sort(Participant[] array)
            {
                if (array == null) return;

                foreach (var participant in array)
                {
                    if (participant._marks == null || participant._places == null) return;
                }

                Array.Sort(array, (participant1, participant2) =>
                {
                    //по сумме мест
                    if (participant1.Score > participant2.Score)
                        return 1;
                    else if (participant1.Score < participant2.Score) return -1;

                    //минимальное место
                    if (participant1._places.Min() > participant2._places.Min())
                        return 1;
                    else if (participant1._places.Min() < participant2._places.Min())
                        return -1;

                    //по сумме очков
                    if (participant1._marks.Sum() > participant2._marks.Sum())
                        return -1;
                    else if (participant1._marks.Sum() < participant2._marks.Sum())
                        return 1;

                    return 0;
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
            public Participant[] Participants
            {
                get
                {
                    if (_participants == null) return null;

                    Participant[] copyParticipants = new Participant[_participants.Length];
                    Array.Copy(_participants, copyParticipants, _participants.Length);

                    return copyParticipants;
                }
            }
            public double[] Moods
            {
                get
                {
                    if (_moods == null) return null;

                    double[] copyMoods = new double[_moods.Length];
                    Array.Copy(_moods, copyMoods, _moods.Length);

                    return copyMoods;
                }
            }

            //методы
            protected abstract void ModificateMood();
            public void Evaluate(double[] marks)
            {
                if (_participants == null || _moods == null || marks == null || _moods.Length != marks.Length) return;

                foreach (var participant in _participants)
                {
                    if (participant.Marks == null) continue;

                    if (participant.Marks.All(mark => mark == 0))
                    {
                        for (int i = 0; i < _moods.Length; i++)
                        {
                            participant.Evaluate(marks[i] * _moods[i]);
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
