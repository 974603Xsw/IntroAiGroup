Student Details:
Adam Guest: 2084678
Jordan Smith: 100568702
Sean Morris: 974603X

Features:

i)	Truth table method working
ii)	Forward chaining method working
iii)Backward chaining method working
iv) All methods only account for the following symbols: "=>", "&" (Implication and AND)

Bugs:
TT and FC work of the same Ask and Tell format made by FIO. BC requires an unformatted Ask and Tell, essentially uses what is in the textfile with no formatting. 

Missing:
v)  These methods do not cover Bi-Conditional ("<=>"), conjunction ("^"), nots ("!"), disjunction ("!^") or parenthesis ("()"). 

Test Cases:
Testing has been accomplished with using the test1.txt file provided, as well as with direct string inputs. 
Some bugs which have been found is if the ask or tell strings have not been formatted correctly  (with spaces or without spaces), some of the methods do not work
as intended. This is seen with TT,FC in comparison to BC. TT and FC can work off one formatted version accomplished by the FIO class. whilst BC requires an unformatted
ask and tell string. 

Acknowledgements:

TT Method:
COS30019: Introduction to Artificial Intelligence, Lecture 7. https://ilearn.swin.edu.au/bbcswebdav/pid-6303217-dt-content-rid-35717180_2/courses/2017-HS1-COS30019-220389/07%20-%20Propositional%20Logic%20-%202spp%281%29.pdf
CS 2710 Foundtations of AI, Lecture 9. https://people.cs.pitt.edu/~milos/courses/cs2710/lectures/Class9.pdf

FC Method:
http://snipplr.com/view/56296/ai-forward-chaining-implementation-for-propositional-logic-horn-form-knowledge-bases/


BC Method:
http://www-personal.umd.umich.edu/~leortiz/teaching/6.034f/Fall05/rules/fwd_bck.pdf
http://snipplr.com/view/56297/ai-backward-chaining-implementation-for-propositional-logic-horn-form-knowledge-bases/

Notes:
Program accepts arguements as specified in the Assignment outline. Method, then file. 

Summary Report:
Adam Guest: Contributed to the FC method. 33.3% Contribution towards assignment
Jordan Smith: Contributed towards the BC method. 33.3% Contribution towards assignment
Sean Morris: Contributed towards TT method. 33.3% Contribution towards assignment