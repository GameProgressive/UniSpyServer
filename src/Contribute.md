```python

class BaseClass:
    """
    We use class static member only to type hint the class instance member
    Do not initialize the class static member
    """
    _property1:type1
    _property2:type2


    def __init__(self):
        # if the property do not have default value it must be initialized as None
        self._property1 = None
        # In the base class we have to check whether the _property has been initialized, if not we init it
        if not hasattr(self,"_property1"):
            self._property2 = value2
```