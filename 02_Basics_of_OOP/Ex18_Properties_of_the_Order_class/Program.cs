using System;

namespace Coding.Exercise
{
    public class Order
    {
        //your code goes here

        public Order(string item, DateTime date)
        {
            Item = item;
            Date = date;
        }

        private DateTime _date;

        public string Item { get; }
        public DateTime Date
        {
            get { return _date; }
            set
            {
                if (value.Year == DateTime.Now.Year)
                {
                    _date = value;
                }
            }
        }
    }
}
