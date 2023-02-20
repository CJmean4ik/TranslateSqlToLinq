using System;
using System.Collections.Generic;
using System.Linq;
using TranslateSqlToLinq.Entitys;

namespace TranslateSqlToLinq
{
    class LinqExecute
    {
        private int _resultant;
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
            if (_binderSourse == null)
                throw new NullReferenceException($"Делегат {_binderSourse.GetType().FullName} не содержит метода для вызова");      
            if (_sourses == null) _sourses = _binderSourse.Invoke();

            _resultant = 0;
            var firstSelection = _sourses
               .Where(w => w.quantity >= 10 && w.quantity <= 160)
               .OrderByDescending(or => or.quantity)
               .Select(s =>
               {

                   int cumulative = CalcCumulativeTotal(_resultant, s.quantity);
                   _resultant = cumulative;
                   return new NewSourseItem( s.Id, s.item, s.quantity, cumulative );
               })
               .Where(w => w.Total <= 160)
               .ToList();
              
            var maxTotalRecord = firstSelection.LastOrDefault();
            if (maxTotalRecord.Total != 160)
            {
                var newElemen = FindingDifference(maxTotalRecord.Total, maxTotalRecord.Id, TypeCondition.FirstCondition);
                firstSelection.Add(newElemen);
            }
            return firstSelection;
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
            if (_binderSourse == null)
                throw new NullReferenceException($"Делегат {_binderSourse.GetType().FullName} не содержит метода для вызова");
          
            if (_sourses == null) _sourses = _binderSourse.Invoke();

            _resultant = 0;
            var secondSelection = _sourses.Where(w => w.quantity <= 10)
                .OrderByDescending(or => or.quantity)
                .Select(s => 
                {
                    int cumulative = CalcCumulativeTotal(_resultant, s.quantity);
                    _resultant = cumulative;
                    return new NewSourseItem( s.Id, s.item, s.quantity, cumulative );
                })
                .Where(w => w.Total <= 40) 
                .ToList();

            var maxTotalObject = secondSelection.LastOrDefault();
            if (maxTotalObject.Total != 40)
            {
                var newElemen = FindingDifference(maxTotalObject.Total, maxTotalObject.Id, TypeCondition.SecondCondition);
                secondSelection.Add(newElemen);
            }
            return secondSelection;
        }

        private int CalcCumulativeTotal(int previousQuantRes, int currentQuant) => previousQuantRes + currentQuant;      
        private NewSourseItem FindingDifference(int maxTotal, int id,TypeCondition condition)
        {
            int result = condition == TypeCondition.FirstCondition ? 160 - maxTotal : 40 - maxTotal;
            int newMaxTotal = maxTotal + result;
                    if (newMaxTotal == 160)
                        return new NewSourseItem(id + 1, _sourses[id].item, result, newMaxTotal);                        
                    if (newMaxTotal == 40)
                        return new NewSourseItem(id + 1, _sourses[id].item, result, newMaxTotal);                         
            return default;
        }
    }
    enum TypeCondition
    {
        FirstCondition,
        SecondCondition
    }

}
