// https://stackoverflow.com/questions/66727966/is-it-practical-to-use-list-find-to-return-class-members-of-type
// Written by kevin-verstraete on StackOverflow

namespace Sample_Base
{
    public class Base 
    {
        public string Member { get; set; }
    }

    public class Foo : Base { }
    public class Bar : Base { }
 }
 
 namespace Sample_ListByType
 {
    // this won't support multiple types of the same
    public class ListByType
    {
        public List<Base> Behaviours => _behaviours.Select(x => x.Value).ToList();
        public Foo GetFoo { get { return GetBehaviour<Foo>(); } }
        public Bar GetBar { get { return GetBehaviour<Bar>(); } }
        
        private T GetBehaviour<T>() where T : Base
        {
            return _behaviours.TryGetValue(typeof(T), out var result) ? result as T : null;
        }

        private Dictionary<Type, Base> _behaviours = new Dictionary<Type, Base>() 
        {
                {typeof(Foo), new Foo() },
                {typeof(Bar), new Bar()}
        };
    }
}

namespace Sample_ListByStringKey
{
    public class ListByStringKey
    {
        public List<Base> Behaviours => _behaviours.Select(x => x.Value).ToList();
        public Foo GetFoo { get { return GetBehaviour<Foo>(); } }
        public Bar GetBar { get { return GetBehaviour<Bar>(); } }
        
        private T GetBehaviour<T>([System.Runtime.CompilerServices.CallerMemberName] string memberName = null) where T : Base
        {
            return _behaviours.TryGetValue(memberName, out var result) ? result as T : null;
        }
        
        private Dictionary<string, Base> _behaviours = new Dictionary<string, Base>()
        {
                {nameof(GetFoo), new Foo() },
                {nameof(GetBar), new Bar()}
        };
    }
}

namespace Sample_ListWithoutFind
{
    public class ListWithoutFind
    {
        public List<Base> Behaviours { get; } = new List<Base>() { new Foo(), new Bar() };
        public Foo GetFoo { get { return Behaviours.OfType<Foo>().FirstOrDefault(); } }
        public Bar GetBar { get { return Behaviours.OfType<Bar>().FirstOrDefault(); } }
    }
}

namespace Sample_ListBasedOnProperties
{
    public class ListBasedOnProperties
    {
        public List<Base> Behaviours => new List<Base>() { GetFoo, GetBar };
        public Foo GetFoo { get; } = new Foo();
        public Bar GetBar { get; } = new Bar();
    }
}
