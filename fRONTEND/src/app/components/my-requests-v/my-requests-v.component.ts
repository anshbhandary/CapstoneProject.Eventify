import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

interface ManagedEvent {
  managedEventId: number;
  vendorId: string;
  eventRequestId: number;
  customerId: string;
  dateOfEvent: string;
  createdAt: string;
}

interface TodoItem {
  toDoItemId: number;
  managedEventId: number;
  description: string;
  dueDate: string;
  vendorId: string;
  customerId: string;
  eventRequestId: number;
  isCompleted: boolean;
  createdAt: string;
  updatedAt: string | null;
  completed?: boolean;
}

interface ApiResponse<T> {
  result: T[];
  isSuccess: boolean;
  message: string | null;
}

@Component({
  selector: 'app-my-request-v',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './my-requests-v.component.html',
  styleUrls: ['./my-requests-v.component.css']
})
export class MyRequestVComponent implements OnInit {
  events: ManagedEvent[] = [];
  isLoading = false;
  errorMessage = '';
  selectedEvent: ManagedEvent | null = null;
  todoItems: TodoItem[] = [];
  isTodoLoading = false;
  showAddTodoForm = false;
  
  newTodo: Omit<TodoItem, 'toDoItemId' | 'isCompleted' | 'createdAt' | 'updatedAt' | 'completed'> = {
    managedEventId: 0,
    description: '',
    dueDate: '',
    vendorId: '',
    customerId: '',
    eventRequestId: 0
  };

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.loadEvents();
  }

  loadEvents(): void {
    this.isLoading = true;
    this.errorMessage = '';
    const vendorId = sessionStorage.getItem('UserId');

    if (!vendorId) {
      this.errorMessage = 'Vendor ID not found. Please log in again.';
      this.isLoading = false;
      return;
    }

    this.http.get<ApiResponse<ManagedEvent>>('https://localhost:5004/api/EventManaging')
      .subscribe({
        next: (response) => {
          this.events = (response.result || []).filter(event => 
            event.vendorId === vendorId
          );
          this.isLoading = false;
        },
        error: (err) => {
          this.errorMessage = 'Failed to load events. Please try again later.';
          this.isLoading = false;
          console.error('Error loading events:', err);
        }
      });
  }

  openTodoPanel(event: ManagedEvent): void {
    if (this.selectedEvent?.managedEventId === event.managedEventId) {
      return;
    }

    this.selectedEvent = event;
    this.resetNewTodo();
    this.loadTodoItems(event);
  }

  closeTodoPanel(): void {
    this.selectedEvent = null;
    this.todoItems = [];
    this.showAddTodoForm = false;
    this.resetNewTodo();
  }

  loadTodoItems(event: ManagedEvent): void {
    this.isTodoLoading = true;
    const requestUrl = `https://localhost:5004/api/ToDoList/Event/${event.managedEventId}`;
  
    this.http.get<ApiResponse<TodoItem>>(requestUrl)
      .subscribe({
        next: (response) => {
          this.todoItems = (response.result || []).map(item => ({
            ...item,
            completed: item.isCompleted
          }));
          this.isTodoLoading = false;
        },
        error: (err) => {
          this.errorMessage = 'Failed to load todo items. Please try again later.';
          this.isTodoLoading = false;
          console.error('Error loading todo items:', err);
        }
      });
  }

  addTodoItem(): void {
    if (!this.newTodo.description || !this.newTodo.dueDate) {
      this.errorMessage = 'Please fill in all required fields.';
      return;
    }

    const todoToAdd = {
      ...this.newTodo,
      isCompleted: false
    };

    this.http.post<{result: TodoItem}>('https://localhost:5004/api/ToDoList/Event', todoToAdd)
      .subscribe({
        next: (response) => {
          if (response.result) {
            this.todoItems = [...this.todoItems, {
              ...response.result,
              completed: response.result.isCompleted
            }];
            this.showAddTodoForm = false;
            this.resetNewTodo();
          }
        },
        error: (err) => {
          this.errorMessage = 'Failed to add todo item. Please try again later.';
          console.error('Error adding todo item:', err);
        }
      });
  }

  updateTodoStatus(todo: TodoItem): void {
    if (!todo.toDoItemId) {
      console.error('No toDoItemId found for the todo item');
      return;
    }
  
    const updateUrl = `https://localhost:5004/api/ToDoList/Event/${todo.toDoItemId}`;
    const updateData = {
      description: todo.description,
      isCompleted: !todo.isCompleted,
      updatedAt: new Date().toISOString()
    };
  
    this.http.put(updateUrl, updateData).subscribe({
      next: () => {
        todo.isCompleted = !todo.isCompleted;
        todo.completed = todo.isCompleted;
      },
      error: (err) => {
        this.errorMessage = 'Failed to update todo status. Please try again later.';
        console.error('Error updating todo status:', err);
      }
    });
  }

  // Helper methods
  isEventActive(eventDate: string): boolean {
    return new Date(eventDate) > new Date();
  }

  isEventSoon(eventDate: string): boolean {
    const diffInDays = this.getDateDiffInDays(eventDate);
    return diffInDays > 0 && diffInDays <= 7;
  }

  isEventUrgent(eventDate: string): boolean {
    const diffInDays = this.getDateDiffInDays(eventDate);
    return diffInDays > 0 && diffInDays <= 2;
  }

  isDueSoon(dueDate: string): boolean {
    const diffInDays = this.getDateDiffInDays(dueDate);
    return diffInDays > 0 && diffInDays <= 2;
  }

  getDateDiffInDays(dateString: string): number {
    const date = new Date(dateString);
    const now = new Date();
    return Math.floor((date.getTime() - now.getTime()) / (1000 * 60 * 60 * 24));
  }

  getTodoCount(eventId: number): number {
    return this.todoItems.filter(item => item.managedEventId === eventId).length;
  }

  getTodayDate(): string {
    const now = new Date();
    now.setMinutes(now.getMinutes() - now.getTimezoneOffset());
    return now.toISOString().slice(0, 16);
  }

  formatDate(dateString: string): string {
    const date = new Date(dateString);
    return date.toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'long',
      day: 'numeric',
      hour: '2-digit',
      minute: '2-digit'
    });
  }

  private resetNewTodo(): void {
    if (this.selectedEvent) {
      this.newTodo = {
        managedEventId: this.selectedEvent.managedEventId,
        description: '',
        dueDate: this.getTodayDate(),
        vendorId: this.selectedEvent.vendorId,
        customerId: this.selectedEvent.customerId,
        eventRequestId: this.selectedEvent.eventRequestId
      };
    } else {
      this.newTodo = {
        managedEventId: 0,
        description: '',
        dueDate: '',
        vendorId: '',
        customerId: '',
        eventRequestId: 0
      };
    }
  }
}