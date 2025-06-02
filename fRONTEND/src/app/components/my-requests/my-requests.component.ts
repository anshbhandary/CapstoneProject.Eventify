import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, AbstractControl, ValidatorFn, ValidationErrors } from '@angular/forms';
import { DatePipe, CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { lastValueFrom } from 'rxjs';
import { ToastrService } from 'ngx-toastr';

interface EventRequest {
  eventRequirementId: number;
  customerId: string;
  eventType: string;
  location: string;
  eventDate: string;
  numberOfGuests: number;
  specialRequests?: string;
  budget?: number;
  description: string;
  status?: string;
}

@Component({
  selector: 'app-my-request',
  templateUrl: './my-requests.component.html',
  styleUrls: ['./my-requests.component.css'],
  providers: [DatePipe],
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
})
export class MyRequestComponent implements OnInit {
  private readonly API_BASE_URL = 'https://localhost:5002/api';

  // Form and state management
  requestForm: FormGroup;
  currentStep = 1;
  totalSteps = 3;
  progressPercentage = 0;
  showForm = false;
  isLoading = false;
  errorMessage = '';
  editingRequest = false;

  // Data storage
  requestList: EventRequest[] = [];

  constructor(
    private fb: FormBuilder,
    private datePipe: DatePipe,
    private http: HttpClient,
    private toastr: ToastrService
  ) {
    this.requestForm = this.fb.group({
      eventRequirementId: [0],
      customerId: [''],
      eventType: ['', Validators.required],
      location: ['', [Validators.required, Validators.minLength(3)]],
      eventDate: ['', [Validators.required, this.futureDateValidator()]],
      numberOfGuests: ['', [Validators.required, Validators.min(1)]],
      specialRequests: [''],
      budget: ['', [Validators.min(0)]],
      description: ['', [Validators.required, Validators.minLength(10)]],
    });
  }

  ngOnInit(): void {
    this.requestForm.valueChanges.subscribe(() => {
      this.updateProgress();
    });

    this.loadEventRequirements();
  }

  // Form validation
  private futureDateValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      if (!control.value) return null;
      const selectedDate = new Date(control.value);
      const today = new Date();
      today.setHours(0, 0, 0, 0);
      return selectedDate >= today ? null : { pastDate: true };
    };
  }

  private updateProgress(): void {
    let completedFields = 0;
    const totalFields = 7;

    if (this.requestForm.get('eventType')?.valid) completedFields++;
    if (this.requestForm.get('location')?.valid) completedFields++;
    if (this.requestForm.get('eventDate')?.valid) completedFields++;
    if (this.requestForm.get('numberOfGuests')?.valid) completedFields++;
    if (this.requestForm.get('specialRequests')?.value) completedFields += 0.5;
    if (this.requestForm.get('budget')?.valid) completedFields++;
    if (this.requestForm.get('description')?.valid) completedFields++;

    this.progressPercentage = Math.min(100, Math.round((completedFields / totalFields) * 100));
  }

  // UI actions
  newRequest(): void {
    this.editingRequest = false;
    this.requestForm.reset({
      eventRequirementId: 0,
      customerId: sessionStorage.getItem('UserId') || '',
    });
    this.showForm = true;
    this.currentStep = 1;
    this.updateProgress();
  }

  closeForm(): void {
    this.showForm = false;
    this.editingRequest = false;
    this.requestForm.reset();
    this.currentStep = 1;
  }

  // Data operations
  private getApiUrl(path: string): string {
    return `${this.API_BASE_URL}${path}`;
  }

  async loadEventRequirements(): Promise<void> {
    this.isLoading = true;
    this.errorMessage = '';

    try {
      const userId = sessionStorage.getItem('UserId');
      if (!userId) {
        throw new Error('User ID not found in session storage');
      }

      const apiUrl = this.getApiUrl(`/Events/customer/${userId}`);
      const response: any = await lastValueFrom(this.http.get(apiUrl));

      if (response.isSuccess) {
        this.requestList = Array.isArray(response.result)
          ? response.result
          : response.result
            ? [response.result]
            : [];

        // Fetch status for each event request
        for (const request of this.requestList) {
          request.status = await this.fetchServiceRequestStatus(request.eventRequirementId);
        }
      } else {
        this.errorMessage = response.message || 'Failed to load event requirements';
      }
    } catch (error) {
      console.error('Error loading event requirements:', error);
      this.errorMessage = 'Failed to load event requirements. Please try again later.';
      this.toastr.error('Failed to load requests', 'Error');
    } finally {
      this.isLoading = false;
    }
  }

  private async fetchServiceRequestStatus(eventRequirementId: number): Promise<string> {
    try {
      const apiUrl = this.getApiUrl(`/ServiceRequest/${eventRequirementId}`);
      const response: any = await lastValueFrom(this.http.get(apiUrl));

      if (response.isSuccess && response.result && response.result.length > 0) {
        return response.result[0].status || 'Pending';
      } else {
        console.warn(`No service request found for eventRequirementId: ${eventRequirementId}`);
        return 'Pending';
      }
    } catch (error) {
      console.error(`Error fetching status for eventRequirementId: ${eventRequirementId}`, error);
      return 'Pending';
    }
  }

  // Form navigation
  isStep1Valid(): boolean {
    return !!this.requestForm.get('eventType')?.valid &&
           !!this.requestForm.get('location')?.valid &&
           !!this.requestForm.get('eventDate')?.valid &&
           !!this.requestForm.get('numberOfGuests')?.valid;
  }

  isStep2Valid(): boolean {
    return !!this.requestForm.get('description')?.valid;
  }

  nextStep(): void {
    if (this.currentStep < this.totalSteps) {
      this.currentStep++;
      this.updateProgress();
    }
  }

  prevStep(): void {
    if (this.currentStep > 1) {
      this.currentStep--;
      this.updateProgress();
    }
  }

  // CRUD operations
  async onSubmit(): Promise<void> {
    if (this.requestForm.valid) {
      try {
        this.isLoading = true;
        const userId = sessionStorage.getItem('UserId');
        if (!userId) {
          throw new Error('User ID not found in session storage');
        }

        // Prepare data for API
        const eventData = {
          ...this.requestForm.value,
          customerId: userId,
          eventDate: new Date(this.requestForm.value.eventDate).toISOString(),
        };

        // Determine if create or update
        const isUpdate = eventData.eventRequirementId !== 0;
        const eventApiUrl = this.getApiUrl('/Events');
        const eventResponse: any = await lastValueFrom(
          isUpdate
            ? this.http.put(eventApiUrl, eventData)
            : this.http.post(eventApiUrl, eventData)
        );

        if (!eventResponse.isSuccess) {
          throw new Error(eventResponse.message || `Failed to ${isUpdate ? 'update' : 'create'} event`);
        }

        // Create service request for new events
        if (!isUpdate) {
          const serviceRequestData = {
            requestId: 0,
            customerId: userId,
            eventRequirementId: eventResponse.result.eventRequirementId,
            requestedAt: new Date().toISOString(),
            status: 'Pending',
          };

          const serviceRequestApiUrl = this.getApiUrl('/ServiceRequest');
          const serviceRequestResponse: any = await lastValueFrom(
            this.http.post(serviceRequestApiUrl, serviceRequestData)
          );

          if (!serviceRequestResponse.isSuccess) {
            throw new Error(serviceRequestResponse.message || 'Failed to create service request');
          }
        }

        this.toastr.success(
          isUpdate ? 'Event updated successfully!' : 'Request submitted successfully!',
          'Success'
        );

        await this.loadEventRequirements();
        this.closeForm();
      } catch (error) {
        console.error('Error submitting form:', error);
        this.toastr.error(
          error instanceof Error ? error.message : 'Unknown error occurred',
          'Error'
        );
      } finally {
        this.isLoading = false;
      }
    } else {
      this.requestForm.markAllAsTouched();
      this.toastr.error('Please fill out all required fields correctly.', 'Form Error');
    }
  }

  editRequest(request: EventRequest): void {
    this.editingRequest = true;

    const formattedRequest = {
      ...request,
      eventDate: this.formatDateForInput(request.eventDate),
    };

    this.requestForm.patchValue(formattedRequest);
    this.showForm = true;
    this.currentStep = 1;
    this.updateProgress();
  }

  async deleteRequest(id: number): Promise<void> {
    const confirmed = confirm('Are you sure you want to delete this request?');
    if (confirmed) {
      try {
        this.isLoading = true;
        const apiUrl = this.getApiUrl(`/Events/${id}`);
        await lastValueFrom(this.http.delete(apiUrl));
        await this.loadEventRequirements();
        this.toastr.success('Request deleted successfully!', 'Success');
      } catch (error) {
        console.error('Error deleting request:', error);
        this.toastr.error('Failed to delete request. Please try again.', 'Error');
      } finally {
        this.isLoading = false;
      }
    }
  }

  // Helper methods
  private formatDateForInput(apiDate: string): string {
    const date = new Date(apiDate);
    return date.toISOString().split('T')[0];
  }

  getStatusClass(status: string | undefined): string {
    switch (status?.toLowerCase()) {
      case 'accepted':
        return 'approved';
      case 'rejected':
        return 'rejected';
      default:
        return 'pending';
    }
  }
}