import { CommonModule, DatePipe, NgClass } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

interface ApiResponse<T> {
  result: T[];
  isSuccess: boolean;
  message: string | null;
}

interface ServiceRequest {
  requestId: number;
  customerId: string;
  eventRequirementId: number;
  requestedAt: string;
  status: string;
}

interface Customer {
  result: {
    customerId: string;
    customerName: string;
    phoneNumber: string;
    address: string;
  };
  isSuccess: boolean;
  message: string | null;
}

interface EventDetails {
  result: {
    eventRequirementId: number;
    customerId: string;
    eventType: string;
    location: string;
    eventDate: string;
    numberOfGuests: number;
    specialRequests: string;
    budget: number;
    description: string;
  };
  isSuccess: boolean;
  message: string | null;
}

interface EventManagingRequest {
  vendorId: string;
  eventRequestId: number;
  customerId: string;
  dateOfEvent: string;
}

interface EventManagingResponse {
  result: {
    managedEventId: number;
    vendorId: string;
    eventRequestId: number;
    customerId: string;
    dateOfEvent: string;
    createdAt: string;
  };
  isSuccess: boolean;
  message: string | null;
}

@Component({
  selector: 'app-vendorhome',
  standalone: true,
  imports: [CommonModule, FormsModule, NgClass, DatePipe],
  templateUrl: './vendor-home.component.html',
  styleUrls: ['./vendor-home.component.css']
})
export class VendorhomeComponent implements OnInit {
  allRequests: ServiceRequest[] = [];
  filteredRequests: ServiceRequest[] = [];
  selectedRequest: ServiceRequest | null = null;
  customerDetails: Customer | null = null;
  eventDetails: EventDetails | null = null;
  
  skeletonArray: number[] = Array(5).fill(0);
  isLoading = true;
  detailsLoading = false;
  searchTerm = '';
  statusFilter = 'all';
  sortBy = 'newest';
  activeTab = 'new';
  processingRequestId: number | null = null;

  private readonly REQUESTS_API = 'https://localhost:5002/api/ServiceRequest';
  private readonly CUSTOMER_API = 'https://localhost:5002/api/Customer';
  private readonly EVENT_API = 'https://localhost:5002/api/Events';
  private readonly EVENT_MANAGING_API = 'https://localhost:5004/api/EventManaging';

  constructor(private http: HttpClient, private router: Router) {}

  ngOnInit(): void {
    this.fetchServiceRequests();
  }

  fetchServiceRequests(): void {
    this.isLoading = true;
    this.http.get<ApiResponse<ServiceRequest>>(this.REQUESTS_API)
      .subscribe({
        next: (response) => {
          this.allRequests = response.result || [];
          this.applyFilters();
          this.isLoading = false;
        },
        error: (error) => {
          console.error('Error fetching service requests:', error);
          this.isLoading = false;
          alert('Failed to load service requests. Please try again.');
        }
      });
  }

  applyFilters(): void {
    let requests = [...this.allRequests];

    if (this.activeTab === 'new') {
      requests = requests.filter(req => req.status === 'Pending');
    } else if (this.activeTab === 'my') {
      requests = requests.filter(req => req.status !== 'Pending');
    }

    if (this.activeTab === 'my' && this.statusFilter !== 'all') {
      requests = requests.filter(req => req.status === this.statusFilter);
    }

    if (this.searchTerm) {
      const term = this.searchTerm.toLowerCase();
      requests = requests.filter(request =>
        request.requestId.toString().includes(term) ||
        request.status.toLowerCase().includes(term) ||
        request.requestedAt.toLowerCase().includes(term)
      );
    }

    this.filteredRequests = requests.sort((a, b) => {
      const dateA = new Date(a.requestedAt).getTime();
      const dateB = new Date(b.requestedAt).getTime();
      return this.sortBy === 'newest' ? dateB - dateA : dateA - dateB;
    });
  }

  viewRequestDetails(request: ServiceRequest): void {
    this.selectedRequest = request;
    this.detailsLoading = true;
    this.customerDetails = null;
    this.eventDetails = null;

    const customerUrl = `${this.CUSTOMER_API}/${request.customerId}`;
    this.http.get<Customer>(customerUrl)
      .subscribe({
        next: (customer) => {
          this.customerDetails = customer;
          this.checkDetailsLoaded();
        },
        error: (error) => {
          console.error('Error fetching customer details:', error);
          this.checkDetailsLoaded();
        }
      });

    const eventUrl = `${this.EVENT_API}/${request.eventRequirementId}`;
    this.http.get<EventDetails>(eventUrl)
      .subscribe({
        next: (event) => {
          this.eventDetails = event;
          this.checkDetailsLoaded();
        },
        error: (error) => {
          console.error('Error fetching event details:', error);
          this.checkDetailsLoaded();
        }
      });
  }

  checkDetailsLoaded(): void {
    if ((this.customerDetails !== null || this.selectedRequest?.customerId === '0') &&
      (this.eventDetails !== null || this.selectedRequest?.eventRequirementId === 0)) {
      this.detailsLoading = false;
    }
  }

  acceptRequest(request: ServiceRequest): void {
    if (this.processingRequestId !== null) return;
    
    this.processingRequestId = request.requestId;
    
    const vendorId = sessionStorage.getItem('UserId');
    if (!vendorId) {
      this.processingRequestId = null;
      alert('Vendor ID not found. Please log in again.');
      return;
    }
  
    const updatedRequest: ServiceRequest = {
      ...request,
      status: 'Accepted'
    };
  
    // First update the request status to "Accepted"
    this.http.put(`${this.REQUESTS_API}`, updatedRequest)
      .subscribe({
        next: () => {
          // Then continue with event managing logic
          if (!this.eventDetails || this.eventDetails.result.eventRequirementId !== request.eventRequirementId) {
            this.fetchEventDetailsForAccept(request, vendorId);
          } else {
            this.createEventManagingRecord(request, vendorId, this.eventDetails.result.eventDate);
          }
        },
        error: (error) => {
          this.processingRequestId = null;
          console.error('Error updating request status to Accepted:', error);
          alert('Failed to update request status. Please try again.');
        }
      });
  }
  

  private fetchEventDetailsForAccept(request: ServiceRequest, vendorId: string): void {
    const eventUrl = `${this.EVENT_API}/${request.eventRequirementId}`;
    this.http.get<EventDetails>(eventUrl)
      .subscribe({
        next: (event) => {
          if (event && event.result) {
            this.createEventManagingRecord(request, vendorId, event.result.eventDate);
          } else {
            this.processingRequestId = null;
            alert('Failed to load event details. Please try again.');
          }
        },
        error: (error) => {
          this.processingRequestId = null;
          console.error('Error fetching event details:', error);
          alert('Failed to load event details. Please try again.');
        }
      });
  }

  private createEventManagingRecord(request: ServiceRequest, vendorId: string, eventDate: string): void {
    const payload: EventManagingRequest = {
      vendorId: vendorId,
      eventRequestId: request.requestId,
      customerId: request.customerId,
      dateOfEvent: eventDate
    };

    this.http.post<EventManagingResponse>(this.EVENT_MANAGING_API, payload)
      .subscribe({
        next: (response) => {
          this.processingRequestId = null;
          if (response.isSuccess) {
            alert('Event successfully created!');
            this.fetchServiceRequests(); // Refresh the list
          } else {
            alert(response.message || 'Failed to create event');
          }
        },
        error: (error) => {
          this.processingRequestId = null;
          console.error('Error creating event:', error);
          alert('Failed to create event. Please try again.');
        }
      });
  }

  rejectRequest(request: ServiceRequest): void {
    if (this.processingRequestId !== null) return;
    
    this.processingRequestId = request.requestId;
    const updatedRequest = { ...request, status: 'Rejected' };
    
    this.http.put(`${this.REQUESTS_API}/${request.requestId}`, updatedRequest)
      .subscribe({
        next: () => {
          this.processingRequestId = null;
          this.fetchServiceRequests();
          alert('Request rejected successfully.');
        },
        error: (error) => {
          this.processingRequestId = null;
          console.error('Error rejecting request:', error);
          alert('Failed to reject request. Please try again.');
        }
      });
  }

  onTabChange(tab: string): void {
    this.activeTab = tab;
    this.applyFilters();
  }

  onStatusFilterChange(): void {
    this.applyFilters();
  }

  onSortChange(): void {
    this.applyFilters();
  }
}