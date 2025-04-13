using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_7
{
    public class Purple_1
    {
        public class Participant
        {
            //конструктор
            public Participant(string name, string surname)
            {
                _name = name;
                _surname = surname;
                _coef = new double[4];
                for (int i = 0; i < 4; i++) _coef[i] = 2.5;
                _marks = new int[4, 7];

                counterJump = 0;
            }

            //поля
            private string _name;
            private string _surname;
            private double[] _coef;
            private int[,] _marks;

            private int counterJump;

            //свойства
            public string Name => _name;
            public string Surname => _surname;
            public double[] Coefs
            {
                get
                {
                    if (_coef == null) return default(double[]);
                    double[] _copyCoef = new double[4];
                    Array.Copy(_coef, _copyCoef, 4);
                    return _copyCoef;
                }
            }
            public int[,] Marks
            {
                get
                {
                    if (_marks == null) return default(int[,]);
                    int[,] _copyMarks = new int[4, 7];
                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 0; j < 7; j++)
                        {
                            _copyMarks[i, j] = _marks[i, j];
                        }
                    }
                    return _copyMarks;
                }
            }
            public double TotalScore
            {
                get
                {
                    if (_marks == null || _coef == null) return 0;

                    double result = 0;
                    for (int jump = 0; jump < 4; jump++)
                    {
                        int iMax = 0, iMin = 0;
                        double score = 0;
                        for (int judge = 1; judge < 7; judge++)
                        {
                            if (_marks[jump, judge] > _marks[jump, iMax]) iMax = judge;
                            if (_marks[jump, judge] < _marks[jump, iMin]) iMin = judge;
                        }
                        for (int judge = 0; judge < 7; judge++)
                        {
                            if (judge != iMax && judge != iMin) score += _marks[jump, judge];
                        }
                        score *= _coef[jump];
                        result += score;
                    }
                    return result;
                }
            }

            //методы
            public void SetCriterias(double[] coefs)
            {
                if (coefs == null || _coef == null) return;
                Array.Copy(coefs, _coef, 4);
            }
            public void Jump(int[] marks)
            {
                if (marks == null || counterJump >= 4 || _marks == null) return;
                for (int judge = 0; judge < 7; judge++)
                {
                    _marks[counterJump, judge] = marks[judge];
                }
                counterJump++;
            }
            public static void Sort(Participant[] array)
            {
                if (array == null) return;

                for (int i = 1, j = 2; i < array.Length;)
                {
                    if (i == 0 || array[i - 1].TotalScore >= array[i].TotalScore)
                    {
                        i = j;
                        j++;
                    }
                    else
                    {
                        (array[i - 1], array[i]) = (array[i], array[i - 1]);
                        i--;
                    }
                }
            }
            public void Print()
            {
                Console.WriteLine($"{_name} {_surname} {TotalScore}");
            }
        }
        public class Judge
        {
            //конструктор
            public Judge(string name, int[] scores)
            {
                _name = name;
                if(scores != null)
                {
                    _scores = new int[scores.Length];
                    Array.Copy(scores, _scores, scores.Length);
                }

                _counterMark = 0;
            }
            //поля
            private string _name;
            private int[] _scores;

            private int _counterMark;
            //свойства
            public string Name => _name;
            //методы
            public int CreateMark()
            {
                if (_scores == null || _scores.Length == 0) return 0;
                if (_counterMark == _scores.Length) _counterMark = 0;
                return _scores[_counterMark++];
            }
            public void Print()
            {
                Console.WriteLine($"Имя: {_name}");
                Console.WriteLine("Оценки:");
                for (int i = 0; i < _scores.Length; i++)
                {
                    Console.Write(_scores[i] + " ");
                }
            }
        }
        public class Competition
        {
            //конструктор
            public Competition(Judge[] judges)
            {
                _participants = new Participant[0];

                if (judges == null) return;
                _judges = new Judge[judges.Length];
                Array.Copy(judges, _judges, judges.Length);
            }
            //поля
            private Judge[] _judges;
            private Participant[] _participants;
            //свойства
            public Judge[] Judges => _judges;
            public Participant[] Participants => _participants;
            //методы
            public void Evaluate(Participant jumper)
            {
                if (jumper == null || _judges == null) return;

                int[] marks = new int[7];
                for (int i = 0; i < 7; i++)
                {
                    if (_judges[i] == null) return;
                    marks[i] = _judges[i].CreateMark();
                }
                jumper.Jump(marks);
            }
            public void Add(Participant participant)
            {
                if (participant == null || _participants == null) return;

                Array.Resize(ref _participants, _participants.Length + 1);
                _participants[_participants.Length - 1] = participant;

                Evaluate(_participants[_participants.Length - 1]);
            }
            public void Add(Participant[] participants)
            {
                if (participants == null) return;

                foreach (var participant in participants)
                {
                    Add(participant);
                }
            }
            public void Sort()
            {
                Participant.Sort(_participants);
            }
        }
    }
}
