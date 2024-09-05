```python

class BaseClass:
    """
    We use class static member to type hint the class instance member
    Do not initialize the class static member
    """
    _property1:type1
    _property2:type2


    def __init__(self):
        # In the base class we have to check whether the _property has been initialized, if not we init it
        if not hasattr(self,"_property1"):
            self._property1 = value1
```