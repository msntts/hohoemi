namespace hohoemi.Model.Comment
{
    public class CommentModel
    {
        public CommentType Type { private set; get; }

        public BehaviorType Behavior { private set; get; }

        public string Sender { private set; get; }

        // Typeによって意味が変わる
        public string Resource { private set; get; }

        public CommentModel(CommentType commentType, BehaviorType behavior, string sender, string resource)
        {
            Type = commentType;
            Behavior = behavior;
            Sender = sender;
            Resource = resource;
        }
    }
}
