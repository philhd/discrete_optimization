using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace TravelingSalesman
{
    public abstract class SolverBase : ISolver
    {
        public event Action DataComplete;

        /// <summary>
        /// Event for INotifyPropertyChanged
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        public TspGraph Graph { get; protected set; }

        public IMoveDecider MoveDecider { get; set; }

        public abstract TspGraph Solve(IEnumerable<Node> nodes);

        protected void RaiseDataComplete()
        {
            if (this.DataComplete != null)
            {
                this.DataComplete();
            }
        }

        public virtual string SolverInfo
        {
            get
            {
                return string.Format("{0}: ", this.GetType().Name);
            }
        }

        /// <summary>
        /// Property change event for INotifyPropertyChanged
        /// </summary>
        /// <param name="info">Name of property that has changed</param>
        protected void NotifyPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
