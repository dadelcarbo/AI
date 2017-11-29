class trade(object):
    """description of class"""
    date = 0
    value = 0
    kind = 0
    def __init__(self, d, v, k):
        self.date = d
        self.value = v
        self.kind = k