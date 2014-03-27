class node:
    def __init__(self,id):
        self.id = id
        self.color = None
        self.neighbors = set()

class edge:
    def __init__(self, start, end):
        self.start = start
        self.end = end

class graph:
    """description of class"""

    def __init__(self, nodes, edgeList):
        self.num_nodes = nodes
        self.edges = edgeList
        self.colors = [0]
        self.color_distribution = {}
        self.nodes = {}
        for i in range(self.num_nodes):
            self.nodes[i] = node(i)

        #add neighbors based on edge list
        for edge in edgeList:
            self.nodes[edge[0]].neighbors.add(edge[1])
            self.nodes[edge[1]].neighbors.add(edge[0])

    def is_feasible(self):
        for edge in self.edges:
            if self.nodes[edge[0]].color == self.nodes[edge[1]].color:
                return False
        return True

    def get_num_violations(self):
        return len(self.get_violations())

    def get_violations(self):
        violations = []
        for edge in self.edges:
            if self.nodes[edge[0]].color == self.nodes[edge[1]].color:
                violations.append(edge)
        return violations

    #colors the graph only using new colors if it has to
    def smart_greedy_color(self):
        for node in self.nodes:
            if self.nodes[node].color != None:
                #already colored
                continue
            color_chosen = False
            #see if we can use any of our current colors
            for color in self.colors:
                if self.can_color(node,color):
                    self.color_node(node, color)
                    color_chosen = True
                    break
            #if we couldn't color this node with the current pallete, add a new one
            if color_chosen != True:
                new_color = self.colors[-1] +1
                self.colors.append(new_color)
                self.color_node(node,new_color)

    def can_color(self, node, color):
        for neighbor in self.nodes[node].neighbors:
            if self.nodes[neighbor].color == color:
                return False
        return True

    #swaps the color of two nodes
    def swap_colors(self,node1,node2):
        color1 = self.nodes[node1].color
        self.nodes[node1].color = self.nodes[node2].color
        self.nodes[node2].color = color1

    def search_neigborhood(self, start_edge):
        moves_made = 0
        current_violations_count = self.get_num_violations()
        #try swapping the violating node with each one in the list
        start_node = start_edge[0]
        for node in self.nodes:
            #don't consider a swap with the violating edge
            if node != start_edge[0] or node != start_edge[1]:
                self.swap_colors(start_node, node)
                new_violations_count = self.get_num_violations()
                if new_violations_count >= current_violations_count:
                    #reverse the swap
                    self.swap_colors(start_node, node)
                else:
                    current_violations_count = new_violations_count
                    moves_made += 1
                    print "made a move! violations now: " + str(current_violations_count)
        return moves_made

    def color_node(self, node, color):
        if self.nodes[node].color != None:
            #remove current color from distribution
            if self.color_distribution.has_key(self.nodes[node].color):
                self.color_distribution[self.nodes[node].color] -= 1
        self.nodes[node].color = color;
        if self.color_distribution.has_key(color):
            self.color_distribution[color] += 1
        else:
            self.color_distribution[color] = 1

    def get_most_used_color(self):
        most_used_count = 0
        most_used = None
        for color in self.colors:
            if self.color_distribution[color] > most_used_count:
                most_used = color
                most_used_count = self.color_distribution[color]
        return most_used

    def remove_color(self,color,new_color):
        if len(self.colors) > 1:
            #clean up memory
            self.colors.remove(color)
            del self.color_distribution[color]
            # reassign color for nodes with the removed color
            for node in self.nodes:
                if self.nodes[node].color == color:
                    self.color_node(node,new_color)

    def show(self):
        for key, value in self.nodes.items():
            print('{} : {} : {}'.format(key, value.color, value.neighbors))

    def show_color_distribution(self):
        for key, value in self.color_distribution.items():
            print('{} : {}'.format(key, value))

    def get_output(self):
        node_colors = []
        for node in self.nodes:
            node_colors.append(self.nodes[node].color)
        output_data = str(self.num_nodes) + ' ' + str(0) + '\n'
        output_data += ' '.join(map(str, node_colors))
        return output_data