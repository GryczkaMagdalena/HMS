import {inject} from 'aurelia-framework';
import {EmployeeTasksService} from '../../../services/employee-tasks-service';
import {TasksService} from "../../../services/tasks-service";
import * as moment from 'moment';

@inject(EmployeeTasksService, TasksService)
export class MainPanel {
  statusChanging;
  allTasks;

  tasksTodo;
  tasksInProgress;
  tasksDone;

  currentTime;
  shift;

  shiftDetails;

  constructor(private employeeTasksService: EmployeeTasksService, private tasksService: TasksService) {
    this.shiftDetails = {};
    this.statusChanging = false;
    this.getTasks();
    this.getCurrentShift();

    this.updateTimeValues();
    setInterval(() => this.updateTimeValues(), 60000);
    setInterval(() => this.calculateShiftDuration(this.shift), 60000);
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

  updateTimeValues() {
    this.currentTime = moment().format("HH:mm");
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

  private getCurrentShift() {
    this.employeeTasksService.getCurrentShift()
      .then(res => {
        this.shift = res;
        this.calculateShiftDuration(this.shift)

      });
  }

  private calculateShiftDuration(shift) {

    if (shift) {
      if (shift.current) {
        this.shiftDetails.actualDuration = this.calculateTimeDifference(moment(shift.currentShift.startTime), moment());
        this.shiftDetails.leftWorkTime = this.calculateTimeDifference(moment(), moment(shift.currentShift.endTime));
        this.shiftDetails.endTime = moment(shift.currentShift.endTime).format('HH:mm');
      }
    }
  }

  private calculateTimeDifference(start, end) {
    let diff = end.diff(start);
    let duration = moment.duration(diff);
    return (duration.hours() + 'h:' + duration.minutes() + 'm');
  }

}
