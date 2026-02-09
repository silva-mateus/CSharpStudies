

                           TODO List  
                 **Ultimate C\# Masterclass Assignment**

##                  **Overview **                                                                                                                                 

| This application is a simple manager of TODOs. Each TODO is simply a  description of a thing to be done (for example, “Order a cake for the birthday party”). Each description must be unique. TODOs can be added, removed, or listed. |  Console App |
| :---- | :---- |

##                 **Main application workflow**    

| When the application starts, it shall print: |
| :---- |
| Hello\! What do you want to do? \[S\]ee all TODOs \[A\]dd a TODO \[R\]emove a TODO \[E\]xit |
| The user must select one of the given **S, A, R, E** options. The selected option can be both uppercase or lowercase, so it doesn’t matter if the user types “S” or “s” \- both should go to the “see all todos” case. The user can keep selecting different options until the option “\[E\]xit” is selected, which will close the application. |

 

##                 **Selecting an option by the user**

| Scenario | User action | Result |
| :---- | :---- | :---- |
| Sunny day | User selects any of the S,s,A,a,R,r,E,e options. | The correct option is handled. After it is finished, the choice of options is printed again (unless the **Exit** option was chosen, which closes the app). |
| Incorrect or empty input | User does not select any option (empty choice), or the selected option is not valid. | “Incorrect input” is printed to the console. Then, the selection of choices is printed again. It is repeated until the user provides the correct input. |

##                 **Option “S” \- See all TODOs**

| A list of all TODOs shall be printed to the console, all prefixed with an index (starting with 1). For example, it could look like this: |
| :---- |
| 1\. Order a cake for the birthday party. 2\. Buy train tickets for the weekend. 3\. Take Lucky to the vet. |
| After the list of TODOs is printed, the application should print again “What do you want to do?” with all available options. |

 

| Scenario | User action | Result |
| :---- | :---- | :---- |
| Sunny day |  | The list of all TODOs is printed to the console. Then, the choice of options is printed again. |
| There are no TODOs at all |  | “No TODOs have been added yet.” is printed to the console. Then, the choice of options is printed again. |

##                 **Option “A” \- Add a TODO**

| After selecting this option, the application shall print: |
| :---- |
| Enter the TODO description: |
| Then, the user must enter a unique, non-empty description of a TODO. Once it is done, the application prints: |
| TODO successfully added: \[DESCRIPTION \] |
| Where the DESCRIPTION should be the TODO description the user provided. A TODO shall be added to the collection of TODOs (it can be seen by selecting the “S \- See all TODOs” option). After a TODOs is added, the application should print again “What do you want to do?” with all available options. |

 

| Scenario | User action | Result |
| :---- | :---- | :---- |
| Sunny day | User inputs a unique, non-empty description of a TODO. | “TODO successfully added: \[DESCRIPTION\]” is printed to the console. TODO is added to the collection. The choice of options is printed again to the console. |
| Empty description | The description the user provided is empty. | “The description cannot be empty.” is printed to the console. No TODO is added. “Enter the TODO description” is printed again. |
| Non-unique description | There is already a TODO with the same description as the user provided. | “The description must be unique.” is printed to the console. No TODO is added. “Enter the TODO description” is printed again. |

##                 **Option “R” \- Remove a TODO**

| After selecting this option, the app should ask: |
| :---- |
| Select the index of the TODO you want to remove: |
| Then, the list of all TODOs shall be printed, similarly as described for the “S \- See all TODOs” option. If there are no TODOs yet, the program should print: |
| No TODOs have been added yet. |
| The user should select a correct index from the given list (please notice that this list is indexed from 1, so when the user selects “1,” the first TODO from this list should be removed). After the correct index is selected, the TODO should be removed, which means it will no longer be shown when the S \- See all TODOs option is selected. Then, the following message is printed: |
| TODO removed: \[DESCRIPTION\]  |
| …where the DESCRIPTION is the description of the TODO that has just been removed. Then the application should print again “What do you want to do?” with all available options. |

 

| Scenario | User action | Result |
| :---- | :---- | :---- |
| Sunny day | User inputs a correct index of a TODO. | “TODO removed: \[DESCRIPTION\]” is printed to the console. TODO is removed from the collection of TODOs. The choice of options is printed again to the console. |
| There are no TODOs at all |  | “No TODOs have been added yet.” is printed to the console. Then, the choice of options is printed again. |
| Empty index | The user input is empty. | “Selected index cannot be empty.” is printed to the console. No TODO is removed. “Select the index of the TODO you want to remove” is printed again. |
| Invalid index  | The index the user inputs is either not a number or it is not a valid index in the collection.  | “The given index is not valid.” is printed to the console. No TODO is removed. “Select the index of the TODO you want to remove” is printed again. |


##                 **Option “E” \- Exit**

| This option simply closes the application. |
| :---- |

                           