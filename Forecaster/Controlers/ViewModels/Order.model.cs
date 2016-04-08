using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Parent = daisybrand.forecaster;

namespace daisybrand.forecaster.Controlers.ViewModels
{
	public class Order:INotifyPropertyChanged
	{
		//  #### Variables #####
		//I	    Safety Stock Days
		//II	# of days per week 852 data is reported
		//III	Count of Days of QS INCLUDING and AFTER order day
		//IV	Days of QS BEFORE and INCLUDING order Arrival Day
		//V	    Days of QS AFTER order arrival at DC (Don't include day of arrival)


		//  ##### Safety Stock Formula ######
					
		//  IF:					
		//      V is Greater or equal to then I
					
		//  TRUE: Week 3 Forecast			
		//      X	(I / II)	
					
		//  False:					
		//      (Week 3 Forecast		X	(V / II))
		//      PLUS	+						
		//      ((I - V) / II) x week 4 forecast			

		#region properties
		private int _WEEK4_FORECAST;
		private int _WEEK3_FORECAST;
		private int _WEEK2_FORECAST;
		private int _CURRENT_WEEK_FOECAST;
		private int _ORDER;
		private int _F;
		private int _E;
		private int _D;
		private int _C;
		private int _B;
		private int _A;
		private int _NUMBER_OF_DAYS_AFTER_ARRIVAL;
		private int _NUMBER_OF_DAYS_BEFORE_AND_ARRIVAL;
		private int _NUMBER_OF_DAYS_AFTER_AND_ORDER_DAY;
		private int _NUMBER_OF_DAYS_PER_852;
		private int _SAFETY_STOCK_DAYS;
		private System.DayOfWeek _ORDER_DAY;

		public int CURRENT_WEEK_FOECAST
		{
			get
			{
				return _CURRENT_WEEK_FOECAST;
			}
			set
			{
				_CURRENT_WEEK_FOECAST = value;
				NotifyPropertyChanged();
			}
		}

		public int WEEK2_FORECAST
		{
			get
			{
				return _WEEK2_FORECAST;
			}
			set
			{
				_WEEK2_FORECAST = value;
				NotifyPropertyChanged();
			}
		}

		public int WEEK3_FORECAST
		{
			get
			{
				return _WEEK3_FORECAST;
			}
			set
			{
				_WEEK3_FORECAST = value;
				NotifyPropertyChanged();
			}
		}

		public int WEEK4_FORECAST
		{
			get
			{
				return _WEEK4_FORECAST;
			}
			set
			{
				_WEEK4_FORECAST = value;
				NotifyPropertyChanged();
			}
		}

		public System.DayOfWeek ORDER_DAY
		{
			get
			{
				return _ORDER_DAY;
			}
			set
			{
				_ORDER_DAY = value;
				NotifyPropertyChanged();
			}
		}

		public int SAFETY_STOCK_DAYS
		{
			get
			{
				return _SAFETY_STOCK_DAYS;
			}
			set
			{
				if (_SAFETY_STOCK_DAYS == value) return;
				_SAFETY_STOCK_DAYS = value;
				//UPDATE THE SETTINGS OBJECT
                
				NotifyPropertyChanged();
			}
		}

		public int NUMBER_OF_DAYS_PER_852
		{
			get
			{
				return _NUMBER_OF_DAYS_PER_852;
			}
			set
			{
				_NUMBER_OF_DAYS_PER_852 = value;
				NotifyPropertyChanged();
			}
		}

		public int NUMBER_OF_DAYS_AFTER_AND_ORDER_DAY
		{
			get
			{
				return _NUMBER_OF_DAYS_AFTER_AND_ORDER_DAY;
			}
			set
			{
				_NUMBER_OF_DAYS_AFTER_AND_ORDER_DAY = value;
				NotifyPropertyChanged();
			}
		}

		public int NUMBER_OF_DAYS_BEFORE_AND_ARRIVAL
		{
			get
			{
				return _NUMBER_OF_DAYS_BEFORE_AND_ARRIVAL;
			}
			set
			{
				_NUMBER_OF_DAYS_BEFORE_AND_ARRIVAL = value;
				NotifyPropertyChanged();
			}
		}

		public int NUMBER_OF_DAYS_AFTER_ARRIVAL
		{
			get
			{
				return _NUMBER_OF_DAYS_AFTER_ARRIVAL;
			}
			set
			{
				_NUMBER_OF_DAYS_AFTER_ARRIVAL = value;
				NotifyPropertyChanged();
			}
		}

		/// <summary>
		/// Pulls from current weeks' (week 1) Forecast still to occur after order day
		/// "Updated" current week forecast X (III / II)
		/// </summary>
		public int A
		{
			get
			{
				return _A;
			}
			set
			{
				_A = value;
				NotifyPropertyChanged();
			}
		}

		/// <summary>
		/// Next week's Forecast (week 2)
		/// </summary>
		public int B
		{
			get
			{
				return _B;
			}
			set
			{
				_B = value;
				NotifyPropertyChanged();
			}
		}


		/// <summary>
		/// Pulls from week 3 forecast to occur Prior to next delivery.
		///Week 3 Forecast X (IV / II)
		/// </summary>
		public int C
		{
			get
			{
				return _C;
			}
			set
			{
				_C = value;
				NotifyPropertyChanged();
			}
		}


		/// <summary>
		/// Safety Stock
		/// (See separate formula for Safety Stock Cases calculation)
		/// </summary>
		public int D
		{
			get
			{
				return _D;
			}
			set
			{
				_D = value;
				NotifyPropertyChanged();
			}
		}

		/// <summary>
		/// On-Hand day of order (852 QA)
		/// </summary>
		public int E
		{
			get
			{
				return _E;
			}
			set
			{
				_E = value;
				NotifyPropertyChanged();
			}
		}


		/// <summary>
		/// On-order day of order (852 QP)
		///(looking into if we currently store QP value)
		/// </summary>
		public int F
		{
			get
			{
				return _F;
			}
			set
			{
				_F = value;
				NotifyPropertyChanged();
			}
		}

		public int ORDER
		{
			get
			{
				return _ORDER;
			}
			set
			{
				_ORDER = A + B + C + D - E - F;
				NotifyPropertyChanged();
			}
		}


		#endregion

		#region INotifyPropertyChanged Members

		public event PropertyChangedEventHandler PropertyChanged;

		private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		#endregion

	}
}
