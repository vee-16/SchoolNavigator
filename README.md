# JCNavigator - School Navigation System

---

## **Documented Design**

### **Overview**
The JCNavigator project is designed to create a Navigation Web Application to assist students, teachers, and visitors in finding the shortest route to their destination. The primary aim is to reduce confusion and delays caused by navigating unfamiliar locations.

Key features include:
- **Shortest Path Algorithm**: Implemented in C# to calculate the shortest route between two points.
- **User Interface**: Built using HTML and CSS to provide a simple and intuitive experience, requiring only start and end locations as inputs.
- **MVC**: The use of MVC (Model-View-Controller) ensures easy maintenance and scalability.

View Testing Videos: https://youtu.be/th6XWlC7j54?feature=shared

---

## **Technologies Used**
- **Language**: C#, HTML, CSS
- **Framework**: MVC 5.0 with ASP.NET Web API
- **Database**: SQL-based `SchoolNavDB` with a composite key structure for unique location identification.
- **Design Patterns**: Object-Oriented Programming (OOP) principles for reusability and encapsulation.

---

## **Framework Details**

### **Model-View-Controller (MVC)**
- **Model**: Manages data logic and interactions with the database.
- **View**: Displays data to users in a visually meaningful format.
- **Controller**: Handles user input, processes it, and returns the appropriate response.

**Advantages of MVC in JCNavigator**:
- Simplifies testing by separating components.
- Supports customization via third-party engines.
- Enhances the manageability of complex projects.

---

## **Data Structure Table**
| **Name**           | **Type**   | **Description**                                                                                             |
|---------------------|------------|-------------------------------------------------------------------------------------------------------------|
| `iRow`             | Array      | Stores x-coordinates for each floor (19 rows).                                                              |
| `iColumns`         | Array      | Stores y-coordinates for each floor (24 columns).                                                           |
| `Ground_Floor.dbo` | Database   | Stores row, column, path status, and location data for the ground floor.                                     |
| `Label`            | Array      | Stores the names of each location.                                                                          |
| `Path`             | Array      | Stores the calculated path coordinates for display.                                                         |

---

## **Identifiers**
| **Identifier Name** | **Type**       | **Data Type** | **Description**                                     | **Default Value** |
|----------------------|----------------|---------------|----------------------------------------------------|-------------------|
| `iRows`             | Constant       | Array         | Default rows for each floor.                      | 19                |
| `iCols`             | Constant       | Array         | Default columns for each floor.                   | 24                |
| `shortest_path`     | Variable       | Array         | Stores the shortest path coordinates.             | Null              |
| `Start`             | Variable/Input | String        | Maps user input start location to coordinates.    | Null              |
| `End`               | Variable/Input | String        | Maps user input end location to coordinates.      | Null              |


---

## **Database Design**
**Database Name**: `SchoolNavDB`

| **Attribute** | **Description**                                                               | **Type**    |
|---------------|-------------------------------------------------------------------------------|-------------|
| `ID`          | Unique identifier for each location.                                          | Integer     |
| `Row`         | Row number (0-19).                                                            | Integer     |
| `Col`         | Column number (0-24).                                                         | Integer     |
| `Value`       | Indicates the type of location (e.g., Path, Room, Exit).                      | Integer     |
| `Name`        | Name of the location (nullable for corridors).                                | Text or Null|
| `PathOrNot`   | Indicates path availability (`0` for available, `1` for unavailable).         | Integer     |

---

## **User Interface**
- **Home Page**: Provides information on how to use the website effectively.
- **Floor Plan Tabs**: Displays individual floor plans (Ground, First, Second).
- **Find Path Page**: Accepts start and end locations via dropdown menus and highlights the calculated route.

---

## **Testing**
### **Data Validation**
| **Test Number** | **Purpose**              | **Test Data** | **Expected Result**                     |
|------------------|--------------------------|---------------|------------------------------------------|
| 1                | Validate row input.     | `7`           | Accepted (Valid Range: 0-19).            |
| 2                | Validate column input.  | `8`           | Accepted (Valid Range: 0-24).            |
| 3                | Validate PathOrNot.     | `0` or `1`    | Accepted.                                |
| 4                | Validate user inputs.   | `Start/End`   | Errors if not selected.                  |

---

## **Controllers and Methods**
### **Controller Actions**
- **Ground_FloorController**, **First_FloorController**, **Second_FloorController**
    - `ViewAll()`
    - `Index()`
    - `Details(int)`
    - `Create()`
    - `Edit(int)`
    - `Delete(int)`

- **HomeController**
    - `SearchAction(string)`
    - `SearchView()`
    - `LoginView()`

---
