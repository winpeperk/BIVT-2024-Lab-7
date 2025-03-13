using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static Lab_6.Purple_1;

namespace Lab_6
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
            public double[] Marks => _marks;
            public int[] Places => _places;
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
                if(participants == null || participants.Length == 0) return;

                foreach (var participant in participants)
                {
                    if (participant._marks == null || participant._places == null) return;
                }

                double[,] matrixMarks = new double[participants.Length, 7];

                for(int i = 0; i < participants.Length; i++)
                {
                    for(int j = 0; j < 7; j++)
                    {
                        matrixMarks[i, j] = participants[i]._marks[j];
                    }
                }

                double[,] copyMatrix = new double [matrixMarks.GetLength(0), 7]; //копируем матрицу

                for (int i = 0; i < copyMatrix.GetLength(0); i++)
                {
                    for (int j = 0; j < 7; j++)
                    {
                        copyMatrix[i, j] = matrixMarks[i, j];
                    }
                } 

                for (int j = 0; j < 7; j++)  //сортировка по убыванию
                {
                    for(int i = 1, k = 2; i < copyMatrix.GetLength(0); )
                    {
                        if(i == 0 || copyMatrix[i - 1, j] >= copyMatrix[i, j])
                        {
                            i = k;
                            k++;
                        }
                        else
                        {
                            (copyMatrix[i - 1, j], copyMatrix[i, j]) = (copyMatrix[i, j], copyMatrix[i - 1, j]);
                            i--;
                        }
                    }
                } 

                double[,] matrixPlaces = new double[matrixMarks.GetLength(0), 7]; //матрица мест
                for (int j = 0; j < 7; j++) 
                {
                    double lastMark = copyMatrix[0, j];
                    int place = 1;
                    matrixPlaces[0, j] = 1;
                    for (int i = 1; i < copyMatrix.GetLength(0); i++)
                    {
                        if (copyMatrix[i, j] != lastMark)
                        {
                            lastMark = copyMatrix[i, j];
                            matrixPlaces[i, j] = place + 1;
                            place++;
                        }
                        else
                        {
                            matrixPlaces[i, j] = matrixPlaces[i - 1, j];
                        }
                    }
                }

                for(int j = 0; j < 7; j++) //присваиваем места исходной матрице
                {
                    for(int i = 0;  i < matrixPlaces.GetLength(0); i++)
                    {

                        for(int ii = 0; ii < matrixPlaces.GetLength(0); ii++)
                        {
                            if (matrixMarks[i, j] == copyMatrix[ii, j])
                            {
                                matrixMarks[i, j] = matrixPlaces[ii, j];
                                break;
                            }
                        }
                    }
                } 

                for (int i = 0; i < matrixMarks.GetLength(0); i++)
                {
                    for(int j = 0; j < 7; j++)
                    {
                        participants[i]._places[j] = (int)matrixMarks[i, j];
                    }
                }

                for(int j = 1, k = 2; j < participants.Length; )
                {
                    if(j == 0 || matrixMarks[j - 1, 6] <= matrixMarks[j, 6])
                    {
                        j = k;
                        k++;
                    }
                    else
                    {
                        (matrixMarks[j - 1, 6], matrixMarks[j, 6]) = (matrixMarks[j, 6], matrixMarks[j - 1, 6]);
                        (participants[j - 1], participants[j]) = (participants[j], participants[j - 1]);
                        j--;
                    }
                } 

            }
            public static void Sort(Participant[] array)
            {
                if (array == null) return;

                foreach(var participant in array)
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
                    else if(participant1._places.Min() < participant2._places.Min())
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
                if(moods == null) return;

                _moods = new double[moods.Length];
                Array.Copy(moods, _moods, _moods.Length);
                ModificateMood();

                _participants = new Participant[0];
            }

            //поля
            private Participant[] _participants;
            protected double[] _moods;

            //свойства
            public Participant[] Participants
            {
                get
                {
                    if (_participants == null) return null;

                    Participant[] copyParticipants = new Participant[ _participants.Length ];
                    Array.Copy(_participants, copyParticipants, _participants.Length );

                    return copyParticipants;
                }
            }
            public double[] Moods
            {
                get
                {
                    if (_moods == null) return null;

                    double[] copyMoods = new double[ _moods.Length ];
                    Array.Copy(_moods, copyMoods, _moods.Length );

                    return copyMoods;
                }
            }

            //методы
            protected abstract void ModificateMood();
            public void Evaluate(double[] marks)
            {
                if(_participants == null || _moods == null || marks == null || _moods.Length != marks.Length) return;

                foreach (var participant in _participants)
                {
                    if(participant.Marks.All(mark => mark == 0))
                    {
                        for(int i = 0; i < _moods.Length; i++)
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
                if(_moods == null) return;

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
                    .Select((mood, judge) => mood * ( 1 + (judge + 1) / 100.0))
                    .ToArray();
            }
        }

    }
}
