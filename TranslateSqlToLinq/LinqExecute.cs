using System;
using System.Collections.Generic;
using System.Linq;
using TranslateSqlToLinq.Entitys;

namespace TranslateSqlToLinq
{
    class LinqExecute
    {      
        private List<SourseItem> _sourses;
        private readonly Func<List<SourseItem>> _binderSourse;

        public LinqExecute(Func<List<SourseItem>> binderSourse)
        {
            _binderSourse = binderSourse;
        }

        /// <summary>
        /// Метод который обрабатывает колекцию по условию №1:
        /// </summary>
        /// <remarks>
        /// Действия: <br />
        /// Получаетет элементы которые больше 10 и меньше 160 <br />
        /// Сортирует по убыванию <br />
        /// Считает нарастающий итог по полю quantity <br />
        /// Выбирает записи, пока нарастаюищй тог меньше или равен 160 <br />
        /// Выполняет перерасчёт, если нарастающий итог не равен 160 
        /// </remarks>
        /// <returns>колекция NewSourseItem</returns>
        /// <exception cref="NullReferenceException"></exception>
        public List<NewSourseItem> GetAllItemsByFirstCondition()
        {
            int cumulative = 0;
            int _resultant = 0;

            if (_binderSourse == null)
                throw new NullReferenceException($"Делегат {_binderSourse.GetType().FullName} не содержит метода для вызова");      
            if (_sourses == null) _sourses = _binderSourse.Invoke();

            var listOrderSourse = new List<NewSourseItem>();
            var firstSelection = _sourses
               .Where(w => w.quantity >= 10 && w.quantity <= 160)
               .OrderByDescending(or => or.quantity)                           
               .ToList();
            foreach (var obj in firstSelection)
            {

                if (cumulative == 160) break;

                cumulative = CalcCumulativeTotal(_resultant, obj.quantity);                 
                if (cumulative <= 160)
                {
                    listOrderSourse.Add(new NewSourseItem(obj.Id, obj.item, obj.quantity, cumulative));
                    _resultant = cumulative;
                    continue;
                }
                else
                {
                    int result = 160 - _resultant;
                    listOrderSourse.Add(new NewSourseItem(obj.Id, obj.item, result, 160));
                    break;
                }
            }

            return listOrderSourse;
        }

        /// <summary>
        /// Метод который обрабатывает колекцию по условию №2:
        /// </summary>
        /// <remarks>
        /// Действия: <br />
        /// Получаетет элементы которые  меньше 10 <br />
        /// Сортирует по возрастанию <br />
        /// Считает нарастающий итог по полю quantity <br />
        /// Выбирает записи, пока нарастаюищй итог меньше или равен 40 <br />
        /// Выполняет перерасчёт, если нарастающий итог не равен 40 
        /// </remarks>
        /// <returns>колекция NewSourseItem</returns>
        /// <exception cref="NullReferenceException"></exception>
        public List<NewSourseItem> GetAllItemsBySecondCondition()
        {
            int cumulative = 0;
            int _resultant = 0;

            if (_binderSourse == null)
                throw new NullReferenceException($"Делегат {_binderSourse.GetType().FullName} не содержит метода для вызова");
          
            if (_sourses == null) _sourses = _binderSourse.Invoke();

            var secondSelection = _sourses.Where(w => w.quantity < 10)
                .OrderByDescending(or => or.quantity)            
                .ToList();

            var listOrderSourse = new List<NewSourseItem>();

            foreach (var obj in secondSelection)
            {
                if (cumulative == 40) break;

                cumulative = CalcCumulativeTotal(_resultant, obj.quantity);
                if (cumulative <= 40)
                {
                    listOrderSourse.Add(new NewSourseItem(obj.Id, obj.item, obj.quantity, cumulative));
                    _resultant = cumulative;
                    continue;
                }
                else
                {
                    int result = 40 - _resultant;
                    listOrderSourse.Add(new NewSourseItem(obj.Id, obj.item, result, 40));
                    break;
                }
            }
            return listOrderSourse;
        }

        private int CalcCumulativeTotal(int previousQuantRes, int currentQuant) => previousQuantRes + currentQuant;      
   
    }
    

}
