# Pager
A little utility library to page an IQueryable collection.  

## How to use  

    int page = 1;
    int itemsPerPage = 30;
    IQueryable<StatusLogs> statusLogs = this.GetLogs();
    IPaging<StatusLogs> pagedLogs = new Paging<StatusLogs>(statusLogs, page, itemsPerPage).Dump();

## Result

  ![Result](http://i.imgur.com/0ZHO9Bc.png)
  
