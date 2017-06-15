import {inject} from 'aurelia-framework';
import {EmployeeTasksService} from '../../../services/employee-tasks-service';
import {TasksService} from "../../../services/tasks-service";

@inject(EmployeeTasksService, TasksService)
export class MainPanel {
  statusChanging;
  allTasks;

  tasksTodo;
  tasksInProgress;
  tasksDone;

  constructor(private employeeTasksService: EmployeeTasksService, private tasksService: TasksService) {
    this.statusChanging = false;
    this.getTasks();
  }

  private filterTasksTodo(allTasks) {
    this.tasksTodo = allTasks.filter(task => task.status === 'Assigned');
  }

  private filterTasksInProgress(allTasks) {
    this.tasksInProgress = allTasks.filter(task => task.status === 'Pending');
  }

  private filterTasksDone(allTasks) {
    this.tasksDone = allTasks.filter(task => task.status === 'Done');
  }

  updateTaskStatus(taskId, status) {
    this.statusChanging = true;
    this.tasksService.updateStatus(taskId, status)
      .then(res => {
        this.getTasks();
      })
      .catch(err => {
        alert("Coś poszło nie tak, status: " + err);
        this.statusChanging = false;
      });
  }

  private getTasks() {
    this.employeeTasksService.getTasks()
      .then(data => {
        this.allTasks = data;
        this.filterTasksTodo(data);
        this.filterTasksInProgress(data);
        this.filterTasksDone(data);
      })
      .then(() => this.statusChanging = false);
  }

}
