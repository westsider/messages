#region Using declarations
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Serialization;
using NinjaTrader.Cbi;
using NinjaTrader.Gui;
using NinjaTrader.Gui.Chart;
using NinjaTrader.Gui.SuperDom;
using NinjaTrader.Gui.Tools;
using NinjaTrader.Data;
using NinjaTrader.NinjaScript;
using NinjaTrader.Core.FloatingPoint;
using NinjaTrader.NinjaScript.DrawingTools;
#endregion

//This namespace holds Indicators in this folder and is required. Do not change it. 
namespace NinjaTrader.NinjaScript.Indicators
{
	public class Messages : Indicator
	{
		private double MinuteCounter = 0;
		private string CurrentMessage = "no message";
		private int MessageIdx = 0;
		private NinjaTrader.Gui.Tools.SimpleFont myFont = new NinjaTrader.Gui.Tools.SimpleFont("Helvetica", 12) { Size = 12, Bold = false };
		private string[] messages = {
			"I don't honor stops when \nthe trade idea is weak.",
			"How can I align with what the market is trying to do,\n not what I’m trying to do?", 
			"Expect 30% losses.", 
			"Look at fewer things with more understanding", 
			"Please help me take my stops.",
			"Fib extension on range if we break the\n1.61 we go up to 2.61 and start a new range", 
			"We reject the Lvn to head back to the Hvn.", 
			"Properly motivated is key, if money is the motivation \nthen how to I execute correctly on losing days?",
			"Do I believe in this setup enough \nto put a stop under it?",
			"If I don't see good cyclic turns,\n I can find key levels plus TickStrike."
		};

		private TextPosition TextPos = TextPosition.Center;
	
		protected override void OnStateChange()
		{
			if (State == State.SetDefaults)
			{
				Description									= @"Enter the description for your new custom Indicator here.";
				Name										= "Messages";
				Calculate									= Calculate.OnBarClose;
				IsOverlay									= true;
				DisplayInDataBox							= true;
				DrawOnPricePanel							= true;
				DrawHorizontalGridLines						= true;
				DrawVerticalGridLines						= true;
				PaintPriceMarkers							= true;
				ScaleJustification							= NinjaTrader.Gui.Chart.ScaleJustification.Right;
				//Disable this property if your indicator requires custom values that cumulate with each new market data event. 
				//See Help Guide for additional information.
				IsSuspendedWhileInactive					= true;
				MessageFontSize								= 14;
				MessageFontColor							= Brushes.WhiteSmoke;
				MessageBackgroundColor						= Brushes.DimGray;
				MessageBackgroundOpacity					= 70;
				MessageLocation								= TextPosition.Center;
				
				DisplayMinutes								= 1;
				MessageDelayMinutes							= 5;
			}
			else if (State == State.Configure)
			{
				AddDataSeries(Data.BarsPeriodType.Minute, 1);
				ClearOutputWindow();
			}
		}
		
		protected override void OnBarUpdate()
		{
			if (CurrentBar < Count -2 ) return;
			if (BarsInProgress == 1) {
				if ( MinuteCounter == 6 ) {
					MinuteCounter = 0;	
				}
				Print("MinuteCounter " + MinuteCounter);
				if ( MinuteCounter == 0 ) {
					StartMessage();
				}
				if ( MinuteCounter == 1 ) {
					HideMessage(); 	
				}
				MinuteCounter += 1;	
			}
			
			if (BarsInProgress == 1) {
				//Draw.TextFixed(this, "MyTextFixed", CurrentMessage, TextPosition.Center);
				myFont.Size = MessageFontSize;
				Draw.TextFixed(this, "myTextFixed", CurrentMessage, MessageLocation, MessageFontColor,
 					 myFont, Brushes.Transparent, MessageBackgroundColor, MessageBackgroundOpacity);
			}	
		}
		
		private void StartMessage() {
			CurrentMessage = messages[MessageIdx];
			Print("StartMessage: " + CurrentMessage + " count " + MessageIdx );
			MessageIdx +=1;
			if ( MessageIdx > messages.Count() - 1) MessageIdx = 0;
		}

		private void HideMessage() {
			CurrentMessage = "";
			// increment message counter
			Print("Hide message " + CurrentMessage + " count " + MessageIdx );
		}
		
		#region Properties
		
		[NinjaScriptProperty]
		[Range(1, int.MaxValue)]
		[Display(Name="MessageFontSize", Order=1, GroupName="Parameters")]
		public int MessageFontSize
		{ get; set; }

		[NinjaScriptProperty]
		[XmlIgnore]
		[Display(Name="MessageFontColor", Order=2, GroupName="Parameters")]
		public Brush MessageFontColor
		{ get; set; }

		[Browsable(false)]
		public string MessageFontColorSerializable
		{
			get { return Serialize.BrushToString(MessageFontColor); }
			set { MessageFontColor = Serialize.StringToBrush(value); }
		}			

		[NinjaScriptProperty]
		[XmlIgnore]
		[Display(Name="Message Background Color", Order=3, GroupName="Parameters")]
		public Brush MessageBackgroundColor
		{ get; set; }

		[Browsable(false)]
		public string MessageBackgroundColorSerializable
		{
			get { return Serialize.BrushToString(MessageBackgroundColor); }
			set { MessageBackgroundColor = Serialize.StringToBrush(value); }
		}
		
		[NinjaScriptProperty]
		[Range(1, int.MaxValue)]
		[Display(Name="MessageBackgroundOpacity", Order=4, GroupName="Parameters")]
		public int MessageBackgroundOpacity
		{ get; set; }
		
		[NinjaScriptProperty]
		[Display(Name="Message Location", Description="Message Location", Order=5, GroupName="Parameters")]
		public TextPosition MessageLocation
		{ get; set; }
		
		// --------------------- duration -----------------------
		
		[NinjaScriptProperty]
		[Range(1, int.MaxValue)]
		[Display(Name="Message Display Time In Minutes", Order=1, GroupName="Duration")]
		public int DisplayMinutes
		{ get; set; }

		[NinjaScriptProperty]
		[Range(1, int.MaxValue)]
		[Display(Name="Minutes Bettween Messages", Order=2, GroupName="Duration")]
		public int MessageDelayMinutes
		{ get; set; }
		#endregion

	}
}

#region NinjaScript generated code. Neither change nor remove.

namespace NinjaTrader.NinjaScript.Indicators
{
	public partial class Indicator : NinjaTrader.Gui.NinjaScript.IndicatorRenderBase
	{
		private Messages[] cacheMessages;
		public Messages Messages(int messageFontSize, Brush messageFontColor, Brush messageBackgroundColor, int messageBackgroundOpacity, TextPosition messageLocation, int displayMinutes, int messageDelayMinutes)
		{
			return Messages(Input, messageFontSize, messageFontColor, messageBackgroundColor, messageBackgroundOpacity, messageLocation, displayMinutes, messageDelayMinutes);
		}

		public Messages Messages(ISeries<double> input, int messageFontSize, Brush messageFontColor, Brush messageBackgroundColor, int messageBackgroundOpacity, TextPosition messageLocation, int displayMinutes, int messageDelayMinutes)
		{
			if (cacheMessages != null)
				for (int idx = 0; idx < cacheMessages.Length; idx++)
					if (cacheMessages[idx] != null && cacheMessages[idx].MessageFontSize == messageFontSize && cacheMessages[idx].MessageFontColor == messageFontColor && cacheMessages[idx].MessageBackgroundColor == messageBackgroundColor && cacheMessages[idx].MessageBackgroundOpacity == messageBackgroundOpacity && cacheMessages[idx].MessageLocation == messageLocation && cacheMessages[idx].DisplayMinutes == displayMinutes && cacheMessages[idx].MessageDelayMinutes == messageDelayMinutes && cacheMessages[idx].EqualsInput(input))
						return cacheMessages[idx];
			return CacheIndicator<Messages>(new Messages(){ MessageFontSize = messageFontSize, MessageFontColor = messageFontColor, MessageBackgroundColor = messageBackgroundColor, MessageBackgroundOpacity = messageBackgroundOpacity, MessageLocation = messageLocation, DisplayMinutes = displayMinutes, MessageDelayMinutes = messageDelayMinutes }, input, ref cacheMessages);
		}
	}
}

namespace NinjaTrader.NinjaScript.MarketAnalyzerColumns
{
	public partial class MarketAnalyzerColumn : MarketAnalyzerColumnBase
	{
		public Indicators.Messages Messages(int messageFontSize, Brush messageFontColor, Brush messageBackgroundColor, int messageBackgroundOpacity, TextPosition messageLocation, int displayMinutes, int messageDelayMinutes)
		{
			return indicator.Messages(Input, messageFontSize, messageFontColor, messageBackgroundColor, messageBackgroundOpacity, messageLocation, displayMinutes, messageDelayMinutes);
		}

		public Indicators.Messages Messages(ISeries<double> input , int messageFontSize, Brush messageFontColor, Brush messageBackgroundColor, int messageBackgroundOpacity, TextPosition messageLocation, int displayMinutes, int messageDelayMinutes)
		{
			return indicator.Messages(input, messageFontSize, messageFontColor, messageBackgroundColor, messageBackgroundOpacity, messageLocation, displayMinutes, messageDelayMinutes);
		}
	}
}

namespace NinjaTrader.NinjaScript.Strategies
{
	public partial class Strategy : NinjaTrader.Gui.NinjaScript.StrategyRenderBase
	{
		public Indicators.Messages Messages(int messageFontSize, Brush messageFontColor, Brush messageBackgroundColor, int messageBackgroundOpacity, TextPosition messageLocation, int displayMinutes, int messageDelayMinutes)
		{
			return indicator.Messages(Input, messageFontSize, messageFontColor, messageBackgroundColor, messageBackgroundOpacity, messageLocation, displayMinutes, messageDelayMinutes);
		}

		public Indicators.Messages Messages(ISeries<double> input , int messageFontSize, Brush messageFontColor, Brush messageBackgroundColor, int messageBackgroundOpacity, TextPosition messageLocation, int displayMinutes, int messageDelayMinutes)
		{
			return indicator.Messages(input, messageFontSize, messageFontColor, messageBackgroundColor, messageBackgroundOpacity, messageLocation, displayMinutes, messageDelayMinutes);
		}
	}
}

#endregion
