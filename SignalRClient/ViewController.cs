using System;
using MonoTouch.Dialog;
using UIKit;

namespace SignalRClient
{
	public partial class ViewController : DialogViewController
	{

		private readonly EntryElement _input;

		private readonly Section _messages;

		private readonly Client _client;


		public ViewController() : base(UITableViewStyle.Grouped, null)
		{

			_input = new EntryElement(null, "Enter message", null)
			{

				ReturnKeyType = UIReturnKeyType.Send

			};

			_messages = new Section();


			Root = new RootElement("Chat Client")
			{
				new Section { _input },
				_messages
			};
			_client = new Client("iOS");
		}

		public override async void ViewDidLoad()
		{
			base.ViewDidLoad();
			await _client.Connect();
			_input.ShouldReturn += () =>
			{
				_input.ResignFirstResponder(true);

				if (string.IsNullOrEmpty(_input.Value))
					return true;
					
				_client.Send(_input.Value);

				_input.Value = "";

				return true;
			};

			_client.OnMessageReceived += (sender, message) =>
				InvokeOnMainThread(() => _messages.Add(new StringElement(message)));


		// Perform any additional setup after loading the view, typically from a nib.
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
		// Release any cached data, images, etc that aren't in us
		}
	}
}
