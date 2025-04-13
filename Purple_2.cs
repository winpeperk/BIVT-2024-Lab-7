using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_7
{
    public class Purple_2
    {
        public struct Participant
        {
            //конструктор
            public Participant(string name, string surname)
            {
                _name = name;
                _surname = surname;
                _marks = new int[5];
                _distance = 0;

                _target = 0;
            }

            //поля
            private string _name;
            private string _surname;
            private int _distance;
            private int[] _marks;

            private int _target;

            //свойства
            public string Name => _name;
            public string Surname => _surname;
            public int Distance => _distance;
            public int[] Marks
            {
                get
                {
                    if (_marks == null) return default(int[]);
                    int[] copyMarks = new int[_marks.Length];
                    Array.Copy(_marks, copyMarks, _marks.Length);
                    return copyMarks;
                }
            }
            public int Result
            {
                get
                {
                    if (_marks == null || _distance == 0) return 0;

                    int result = _marks.Sum() - _marks.Max() - _marks.Min() + 60 + (_distance - _target) * 2;

                    if (result < 0) return 0;

                    return result;
                }
            }

            //методы
            public void Jump(int distance, int[] marks, int target)
            {
                _distance = distance;
                _target = target;

                if (marks == null || _marks == null || marks.Length != 5) return;
                Array.Copy(marks, _marks, 5);
            }
            public static void Sort(Participant[] array)
            {
                if (array == null) return;

                for (int i = 1, j = 2; i < array.Length;)
                {
                    if (i == 0 || array[i - 1].Result >= array[i].Result)
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
                Console.WriteLine($"{_name} {_surname} {Result}");
            }
        }
        public abstract class SkiJumping
        {
            //конструктор
            public SkiJumping(string name, int standart)
            {
                _name = name;
                _standard = standart;
                _participants = new Participant[0];
            }

            //поля
            private string _name;
            private int _standard;
            private Participant[] _participants;

            //свойства
            public string Name => _name;
            public int Standard => _standard;
            public Participant[] Participants
            {
                get
                {
                    if (_participants == null) return null;

                    Participant[] copyParticipants = new Participant[_participants.Length];
                    Array.Copy(_participants, copyParticipants, copyParticipants.Length);
                    return copyParticipants;
                }
            }

            //методы
            public void Add(Participant participant)
            {
                if (_participants == null) return;

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
            public void Jump(int distance, int[] marks)
            {
                if (_participants == null) return;

                for (int i = 0; i < _participants.Length; i++)
                {
                    if (_participants[i].Distance == 0)
                    {
                        _participants[i].Jump(distance, marks, _standard);
                        break;
                    }
                }
                
            }
            public void Print()
            {
                if (_participants == null) return;
                Console.WriteLine($"Соревнование: {_name}");
                Console.WriteLine($"Норматив дистанции: {_standard}");
                Console.WriteLine("Участники соревнования:");
                foreach (var participant in _participants)
                {
                    participant.Print();
                }
            }
        }
        public class JuniorSkiJumping : SkiJumping
        {
            public JuniorSkiJumping() : base("100m", 100) { }
        }
        public class ProSkiJumping : SkiJumping
        {
            public ProSkiJumping() : base("150m", 150) { }
        }
    }
}

