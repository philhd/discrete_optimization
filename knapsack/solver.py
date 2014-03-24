#!/usr/bin/python
# -*- coding: utf-8 -*-

from collections import namedtuple
import copy
import heapq
Item = namedtuple("Item", ['index', 'value', 'weight','value_density'])
EstimateNode = namedtuple("EstimateNode", ['estimate', 'node'])

debug = False

import time

class Timer(object):
    def __init__(self, verbose=False):
        self.verbose = verbose

    def __enter__(self):
        self.start = time.time()
        return self

    def __exit__(self, *args):
        self.end = time.time()
        self.secs = self.end - self.start
        self.msecs = self.secs * 1000  # millisecs
        if self.verbose:
            print 'elapsed time: %f ms' % self.msecs

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
        self.estimate = self.get_estimate_simple()

    # simple relaxation
    def get_estimate_simple(self):
        estimate = self.value
        room_left = self.room
        #first add up what we already have in the bag
        #for i in range(len(self.decision_vector)):
        #    decision = self.decision_vector[i]
        #    item_value = self.items[i].value * decision
        #    estimate += item_value

        #now take EVERYTHING else starting from the end of our decisions
        for i in range(len(self.decision_vector),len(self.items)):
            if room_left - self.items[i].weight >= 0:
                #take the whole item
                estimate += self.items[i].value
                room_left -= self.items[i].weight
            else:
                #take a piece of the item that will fit
                estimate += self.items[i].value_density * room_left
                break
        return estimate

    def get_estimate_linear_relaxation(self):
        estimate = self.value
        #now take everything else we can fit, and a fraction of the last item
        for i in range(len(self.decision_vector),len(self.items)):
            estimate += self.items[i].value
        return estimate

    def show(self):
        print ' value: ' + str(self.value) + ' room: ' + str(self.room) + ' estimate: ' + str(self.estimate) + ' index: ' + str(self.index) + ' decisions: ' + str(self.decision_vector)

class bb_edge:
    def __init__(self,item_index,take,start,end):
        self.item_index = item_index
        self.take = take
        self.start = None
        self.end = None

#@profile
def solve_bb(item_count, capacity, ordered_items, taken):
    #not going to fill the list as we go, clear it
    while len(taken) != 0:
        del taken[0]

    if debug:
        print ordered_items
    items = ordered_items
#    items = sorted(ordered_items, key = lambda x: x.value_density)
#    items.reverse()
    id_to_index_map = {}
    for i in range(len(items)):
        id_to_index_map[items[i].index] = i
    if debug:
        print items

    #first run greedy algorithm to get a baseline best score.
    room_to_go = capacity
    initial_score = 0
    for item in items:
        if item.weight > capacity:
            #ignore item
            continue

        if room_to_go - item.weight >= 0:
            room_to_go -= item.weight
            initial_score += item.value
        else:
            break

    best_score = initial_score

    if debug:
        print "inital best: " + str(best_score)

    nodes = []
    visited_nodes = []
    best_node = None
    best_index = 0;
    root = bb_node(0,capacity,-1,items,[])
    best_node = root
    vnr = EstimateNode(root.estimate*-1, root)
    #nodes.append(vnr)
    heapq.heappush(nodes,vnr)
    while len(nodes) > 0:
        #with Timer() as a:
        #with Timer() as t:
        #nodes.sort(key = lambda x: x.estimate)
        #print "=> elapsed nodes.sort: %s s" % t.secs
        if debug:
            print "size: " + str(len(nodes))
        #node = nodes.pop().node
        node = heapq.heappop(nodes).node
        if debug:
            node.show()
        index = node.index + 1
        if index < len(items):
            for decision in range(2):
                #need to deep copy the decision vector
                dv_copy1 = copy.copy(node.decision_vector)
                dv_copy1.append(decision)
                room = node.room - items[index].weight*decision
                if room >= 0:
                    child_node = bb_node(node.value + items[index].value*decision,room, index, items, dv_copy1)
                    visited_nodes.append(child_node)
                    if child_node.value >= best_score:
                        #best_score = child_node.value
                        #best_index = len(visited_nodes) - 1
                        best_node = child_node
                        best_score = best_node.value
                        if debug:
                            print 'new best value: ' + str(best_node.value)
                    if child_node.estimate >= best_score:
                        #nodes.append(EstimateNode(child_node.estimate, child_node))
                        heapq.heappush(nodes, EstimateNode(child_node.estimate*-1, child_node))
                    else:
                        if debug:
                            print "pruned: estimate worse than best value"
                else:
                    if debug:
                        print "pruned: infeasible"
        #print "=> elapsed loop: %s s" % t.secs

    print 'nodes visited: ' + str(len(visited_nodes))
    #taken.extend(visited_nodes[best_index].decision_vector)
    for item in ordered_items:
        #taken.append(visited_nodes[best_index].decision_vector[item.index])
        taken.append(best_node.decision_vector[id_to_index_map[item.index]])
    return best_node.value
    #root.show()

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
        items.append(Item(i-1, int(parts[0]), int(parts[1]), float(int(parts[0]))/float(int(parts[1]))))

    taken = [0]*len(items)

    #value = solve_dp(item_count, capacity, items, taken)
    #with Timer() as t:
    value = solve_bb(item_count, capacity, items, taken)
    #print "=> elapsed: %s s" % t.secs

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

