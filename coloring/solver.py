#!/usr/bin/python
# -*- coding: utf-8 -*-
from graph import graph

debug = False

def local_search_1(my_graph):
    continue_search = True

    #now remove colors and use local search to remove violations
    while continue_search:
        #remove a color
        my_graph.remove_color(my_graph.colors[-1])
        violations_after_color_removal = my_graph.get_num_violations()
        print "violations_after_color_removal: " + str(violations_after_color_removal)

        #if somehow removing the color resulted in no violations, skip right to removing another
        if violations_after_color_removal == 0:
            continue

        #iterate local search until we remove violations
        tries_left = 10
        while tries_left > 0:
            violations_before_search = my_graph.get_num_violations()
            print "violations_before_search: " + str(violations_before_search)
            violations = my_graph.get_violations()
            print "violating edges: " + str(violations)
            local_search_moves = my_graph.search_neigborhood(violations[0])
            print "local search moves: " + str(local_search_moves)
            violations_after_search = my_graph.get_num_violations()
            print "violations_after_search: " + str(violations_after_search)
            if violations_after_search == 0:
                break
            else:
                tries_left -= 1

        if tries_left == 0:
            continue_search = False

def solve_it(input_data):
    # Modify this code to run your optimization algorithm

    # parse the input
    lines = input_data.split('\n')

    first_line = lines[0].split()
    node_count = int(first_line[0])
    edge_count = int(first_line[1])

    edges = []
    for i in range(1, edge_count + 1):
        line = lines[i]
        parts = line.split()
        edges.append((int(parts[0]), int(parts[1])))

    my_graph = graph(node_count,edges)

    #use a greedy coloring to get our initial configuration
    if debug: print "violations: " + str(my_graph.get_num_violations())
    my_graph.smart_greedy_color()
    if debug: print "violations: " + str(my_graph.get_num_violations())
    if debug: print "colors used: " + str(len(my_graph.colors))

    violations = my_graph.get_num_violations()

    max_iterations = 10000
    iteration_count = 0
    while violations == 0 and iteration_count < max_iterations:
        if debug: my_graph.show_color_distribution()
        if debug: print "most_used_color: " + str(my_graph.get_most_used_color())
        #local_search_1(my_graph)
        my_graph.remove_color(my_graph.get_most_used_color(),None)
        my_graph.smart_greedy_color()
        violations = my_graph.get_num_violations()
        if debug: print "violations: " + str(violations)
        if debug: print "colors used: " + str(len(my_graph.colors))
        iteration_count += 1

    # build a trivial solution
    # every node has its own color
    solution = range(0, node_count)

    # prepare the solution in the specified output format
    output_data = str(node_count) + ' ' + str(0) + '\n'
    output_data += ' '.join(map(str, solution))

    #return output_data
    return my_graph.get_output()


import sys

if __name__ == '__main__':
    if len(sys.argv) > 1:
        file_location = sys.argv[1].strip()
        input_data_file = open(file_location, 'r')
        input_data = ''.join(input_data_file.readlines())
        input_data_file.close()
        print solve_it(input_data)
    else:
        print 'This test requires an input file.  Please select one from the data directory. (i.e. python solver.py ./data/gc_4_1)'

