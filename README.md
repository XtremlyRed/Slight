# Easy  Library

easy library to development dotnet application


# Usage

##### ViewModel 

ViewModel in mvvm binding 
```
    public class DisplayViewModel:ViewModelBase
    { 
         public int DisplayValue
         {
             get=>GetValue(1);
             set=>SetValue(value);
         }

         private int displayValue;
         public int DisplayValue
         {
             get=>displayValue;
             set=>SetValue(ref displayValue,value);
         }

         public ICommand ShowCommand=>RelayCommand.Bind(()=>
         {
              DisplayValue += DisplayValue; 
         }); 
    }
```

##### Command 

command to mvvm binding 
```
    public ICommand ShowCommand=>RelayCommand.Bind(()=>
    {
         //command logic 
    }); 
```

##### Messenger 

messenger communicate 
```
    var messenger = new Messenger();
    messenger.Subscribe(this, "test_token", () => 
    {
        //logic 
    });

    messenger.Publish("test_token");
    
    messenger.Subscribe<int, object, string[]>(this, "test_token2", (arg1,arg2,arg3) =>
    {
        //arg1 is int value
        //arg2 is object value
        //arg3 is stringp[] value
    
        //logic 
    });

    messenger.Publish("test_token2",1,new object(),new string[] { });
    
```

##### Math Extensions 

math extensions 
```
    int intValue = 100;
    
    intValue.FromRange(90,200);
    
    var inResult = intValue.InRange(10,50);
```


##### annular pool 

annular pool
```
    var pool =new AnnularPool<byte>(); 
    pool.Write(new byte[5], 0, 5);
    pool.Read(new byte[5], 0, 5) 
```