using NUnit.Framework;
using RefactoringEssentials.CSharp.CodeRefactorings;

namespace RefactoringEssentials.Tests.CSharp.CodeRefactorings
{
    [TestFixture]
    public class CreateDelegateTests : CSharpCodeRefactoringTestBase
    {
        
        [Test]
        [Description("Test that only event field declarations are refactored.")]
        public void TestEventDeclarationsAreNotRefactored()
        {
            TestWrongContext<CreateDelegateAction>(
@"interface TestClass
{
    public event $EventHandler MouseUp
    {
        add { AddEventHandler(mouseUpEventKey, value); }
        remove { RemoveEventHandler(mouseUpEventKey, value); }
    }
}
");
        }

        [Test]
        [Description("Test that only event field declarations are refactored.")]
        public void TestDelegateNotCreatedIfAlreadyExists()
        {
            TestWrongContext<CreateDelegateAction>(
@"delegate void MyEventHandler(object sender, System.EventArgs e);

interface TestClass
{
    event $MyEventHandler $evt;
}
");
        }
        
        [TestCase(@"$event MyEventHandler evt;")]
        [TestCase(@"event $MyEventHandler evt;")]
        [TestCase(@"event MyEventHandler $evt;")]
        [Description("Test that an undefined delegate gets created.")]
        public void TestCreateDelegate(string given)
        {
            Test<CreateDelegateAction>(
@"class TestClass
{
	" + given + @"
}
",
@"delegate void MyEventHandler(object sender, System.EventArgs e);

class TestClass
{
	event MyEventHandler evt;
}
");
        }

        [Test]
        [Description("Test that an undefined delegate gets created for interface type.")]
        public void TestCreateDelegateForInterface()
        {
            Test<CreateDelegateAction>(
@"interface TestClass
{
	event MyEventHandler $evt;
}
",
@"delegate void MyEventHandler(object sender, System.EventArgs e);

interface TestClass
{
	event MyEventHandler evt;
}
");
        }

        [Test]
        [Description("Test that an undefined delegate gets created for struct type.")]
        public void TestCreateDelegateForStruct()
        {
            Test<CreateDelegateAction>(
@"struct TestClass
{
	event MyEventHandler $evt;
}
",
@"delegate void MyEventHandler(object sender, System.EventArgs e);

struct TestClass
{
	event MyEventHandler evt;
}
");
        }

        [Test]
        [Description("Test that an undefined delegate gets created as external public.")]
        public void TestCreateDelegateExternalPublic()
        {
            Test<CreateDelegateAction>(
@"public class TestClass
{
    public $event MyEventHandler evt;
}",
@"public delegate void MyEventHandler(object sender, System.EventArgs e);

public class TestClass
{
    public event MyEventHandler evt;
}");
        }

        [Test]
        [Description("Test that an undefined delegate gets created as external public.")]
        public void TestCreateDelegateExternalProtected()
        {
            Test<CreateDelegateAction>(
@"public class TestClass
{
    protected $event MyEventHandler evt;
}",
@"public delegate void MyEventHandler(object sender, System.EventArgs e);

public class TestClass
{
    protected event MyEventHandler evt;
}");
        }

        [Test]
        [Description("Test that an undefined delegate gets created as internal private.")]
        public void TestCreateDelegatePrivate()
        {
            Test<CreateDelegateAction>(
@"public class TestClass
{
    private $event MyEventHandler evt;
}",
@"delegate void MyEventHandler(object sender, System.EventArgs e);

public class TestClass
{
    private event MyEventHandler evt;
}");
        }

        [Test]
        [Description("Test that an undefined delegate gets created as internal private.")]
        public void TestCreateDelegateInteral()
        {
            Test<CreateDelegateAction>(
@"public class TestClass
{
    internal $event MyEventHandler evt;
}",
@"delegate void MyEventHandler(object sender, System.EventArgs e);

public class TestClass
{
    internal event MyEventHandler evt;
}");
        }

        [Test]
        [Description("Test that an undefined delegate gets created and properly uses the static modifier.")]
        public void TestCreatePublicStaticDelegate()
        {
            Test<CreateDelegateAction>(
@"public class TestClass
{
    public static event $MyEventHandler evt;
}",
@"public delegate void MyEventHandler(object sender, System.EventArgs e);

public class TestClass
{
    public static event MyEventHandler evt;
}");
        }


        [TestCase(@"$event MyEventHandler evt;")]
        [TestCase(@"event $MyEventHandler evt;")]
        [TestCase(@"event MyEventHandler $evt;")]
        [Description("Test that an undefined delegate gets created within namespace.")]
        public void TestCreateDelegateWithNamespace(string given)
        {
            Test<CreateDelegateAction>(
@"namespace foo
{
    class TestClass
    {
        " + given + @"
    }
}",
@"namespace foo
{
    delegate void MyEventHandler(object sender, System.EventArgs e);

    class TestClass
    {
        event MyEventHandler evt;
    }
}");
        }

        [TestCase(@"$event EventHandler evt;")]
        [TestCase(@"event $EventHandler evt;")]
        [TestCase(@"event EventHandler $evt;")]
        [TestCase(@"$event EventHandler<AssemblyLoadEventArgs> evt;")]
        [TestCase(@"event $EventHandler<AssemblyLoadEventArgs> evt;")]
        [TestCase(@"event EventHandler<AssemblyLoadEventArgs> $evt;")]
        [Description("Test that a delegate does not get created for standard EventHandler and EventHandler<> types.")]
        public void TestDoesNotCreateDelegate(string given)
        {
            TestWrongContext<CreateDelegateAction>(
@"using System;

namespace foo
{
    class TestClass
    {
        " + given + @"
    }
}");
        }
    }
}
