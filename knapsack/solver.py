#!/usr/bin/python
# -*- coding: utf-8 -*-

from collections import namedtuple
Item = namedtuple("Item", ['index', 'value', 'weight'])

debug = True

class matrix:
    
    # implements basic operations of a matrix class
    
    def __init__(self, value):
        self.value = value
        self.dimx = len(value)
        self.dimy = len(value[0])
        if value == [[]]:
            self.dimx = 0
    
    def zero(self, dimx, dimy):
        # check if valid dimensions
        if dimx < 1 or dimy < 1:
            raise ValueError, "Invalid size of matrix"
        else:
            self.dimx = dimx
            self.dimy = dimy
            self.value = [[0 for row in range(dimy)] for col in range(dimx)]
    
    def identity(self, dim):
        # check if valid dimension
        if dim < 1:
            raise ValueError, "Invalid size of matrix"
        else:
            self.dimx = dim
            self.dimy = dim
            self.value = [[0 for row in range(dim)] for col in range(dim)]
            for i in range(dim):
                self.value[i][i] = 1
    
    def show(self):
        for i in range(self.dimx):
            print self.value[i]
        print ' '

    def __repr__(self):
        return repr(self.value)

class dpSolver:
    def __init__(self,initial_table):
        self.value = value
        self.table = initial_table

    def trace_back():
        print 'blah'

def solve_dp(item_count, capacity, items, taken):
    # Dynamic programming algorithm
    #################################################
    #initialize table
    table = matrix([[]])
    table.zero(capacity,item_count)
    if debug:
        table.show()
        print items
    # calculate column
    for itemIndex in range(0,item_count):
        for current_capacity in range(1,capacity+1):
            if debug:
                print "x: " + str(itemIndex) + " y: " + str(current_capacity-1)

            if itemIndex == 0:
                left_value = 0
            else:
                left_value = table.value[current_capacity-1][itemIndex-1]
            
            if debug:
                print 'left_value: ' + str(left_value)
            # will this item and the remaining capacity 
            # give us enough value to make it worth taking?
            remaining_capacity = current_capacity - items[itemIndex].weight

            # only continue checking if this item will fit
            if remaining_capacity >= 0:
                if remaining_capacity == 0:
                    # nothing else can fit, we're done
                    remaining_value = 0
                elif remaining_capacity > 0:
                    # lookup the remaining value in the table
                    remaining_value = table.value[remaining_capacity-1][itemIndex-1]

                total_value = remaining_value + items[itemIndex].value
                if debug:
                    print "r_c: " + str(remaining_capacity) + " r_v: " + str(remaining_value) + " t_v: " + str(total_value) 
                if total_value > left_value:
                    # take it!
                    table.value[current_capacity-1][itemIndex] = total_value
                    continue

            #if we can't or aren't going to take it, inherit table item from the left
            table.value[current_capacity-1][itemIndex] = left_value
    
    # out optimal value is at the bottom right of the table
    value = table.value[capacity-1][item_count-1] 

    if debug:
        print "best value: " + str(value)
    # now trace back to find out what's in the bag!
    y_index = capacity-1
    x_index = item_count-1

    if debug:
        print taken

    while x_index >= 0 and y_index >= 0:
        if debug:
            print "tb: x: " + str(x_index) + " y: " + str(y_index)
        if x_index <= 0:
            left_value = 0
        else:
            left_value = table.value[y_index][x_index-1]
        if debug:
                print 'left_value: ' + str(left_value)
        #is left value different than current value?
        if left_value != table.value[y_index][x_index]:
            #this means we took the item
            taken[x_index] = 1
            # subtract capacity for this item and move by that much up the table
            y_index -= items[x_index].weight
            x_index -= 1
        else:
            x_index -= 1
            
    if debug:
        table.show()
        print taken

    return value

class bb_node:
    def __init__(self,value,room,index,items,decision_vector):
        self.value = value
        self.room = room
        self.items = items
        self.index = index
        self.decision_vector = decision_vector
        self.estimate = self.get_estimate()

    # simple relaxation
    def get_estimate(self):
        estimate = 0
        #first add up what we already have in the bag
        for i in range(len(self.decision_vector)):
            estimate += self.decision_vector[i]*self.items[self.decision_vector[i]].value

        #now take EVERYTHING else starting from the end of our decisions
        for i in range(len(self.decision_vector),len(self.items)):
            estimate += self.items[i].value

    def show(self):
        print 'value: ' + str(self.value) + ' room: ' + str(self.room) + ' estimate: ' + str(self.estimate)

class bb_edge:
    def __init__(self,item_index,take,start,end):
        self.item_index = item_index
        self.take = take
        self.start = None
        self.end = None

def solve_bb(item_count, capacity, items, taken):
    #get the total optimistic capacity
    max_estimate = 0;
    for i in range(len(items)):
        max_estimate += items[i].value

    root = bb_node(0,capacity,0,items,[])
    root.show()

def solve_it(input_data):
    # Modify this code to run your optimization algorithm

    # parse the input
    lines = input_data.split('\n')

    firstLine = lines[0].split()
    item_count = int(firstLine[0])
    capacity = int(firstLine[1])

    if debug:
        print "item count: " + str(item_count)
        print "capacity: " + str(capacity)

    items = []

    for i in range(1, item_count+1):
        line = lines[i]
        parts = line.split()
        items.append(Item(i-1, int(parts[0]), int(parts[1])))

    taken = [0]*len(items)

    #value = solve_dp(item_count, capacity, items, taken)
    value = solve_bb(item_count, capacity, items, taken)

    #################################################

    # a trivial greedy algorithm for filling the knapsack
    # it takes items in-order until the knapsack is full
    #value = 0
    #weight = 0
    #taken = [0]*len(items)

    #for item in items:
    #    if weight + item.weight <= capacity:
    #        taken[item.index] = 1
    #        value += item.value
    #        weight += item.weight
    
    ## prepare the solution in the specified output format
    output_data = str(value) + ' ' + str(0) + '\n'
    output_data += ' '.join(map(str, taken))
    return output_data

import sys

if __name__ == '__main__':
    if len(sys.argv) > 1:
        file_location = sys.argv[1].strip()
        input_data_file = open(file_location, 'r')
        input_data = ''.join(input_data_file.readlines())
        input_data_file.close()
        print solve_it(input_data)
    else:
        print 'This test requires an input file.  Please select one from the data directory. (i.e. python solver.py ./data/ks_4_0)'

