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

    #colors the graph only using new colors if it has to
    def smart_greedy_color(self):
        for node in self.nodes:
            color_chosen = False
            #see if we can use any of our current colors
            for color in self.colors:
                if self.can_color(node,color):
                    self.nodes[node].color = color;
                    color_chosen = True
                    break
            #if we couldn't color this node with the current pallete, add a new one
            if color_chosen != True:
                new_color = self.colors[-1] +1
                self.colors.append(new_color)
                self.nodes[node].color = new_color

    def can_color(self, node, color):
        for neighbor in self.nodes[node].neighbors:
            if self.nodes[neighbor].color == color:
                return False
        return True

    def show(self):
        #color_string = ''
        #for node in self.nodes:
        #    color_string = color_string + str(self.nodes[node].color) + " "
        #color_string.strip;
        #print str(self.num_nodes) + " 0\n" + color_string
        print self.get_output()

    def get_output(self):
        node_colors = []
        for node in self.nodes:
            node_colors.append(self.nodes[node].color)
        output_data = str(self.num_nodes) + ' ' + str(0) + '\n'
        output_data += ' '.join(map(str, node_colors))
        return output_data