# Database Schema

## Entity Relationship Diagram
```mermaid
erDiagram
    User ||--o{ UserPackage : owns
    User ||--o{ Booking : makes
    User ||--o{ WaitlistEntry : joins
    Country ||--o{ User : resides_in
    Country ||--o{ Package : offered_in
    Country ||--o{ ClassSchedule : hosted_in
    Package ||--o{ UserPackage : purchased_as
    ClassSchedule ||--o{ Booking : has
    ClassSchedule ||--o{ WaitlistEntry : has
    UserPackage ||--o{ Booking : used_for
    
    User {
        int Id PK
        string Email UK
        string PasswordHash
        string FirstName
        string LastName
        int CountryId FK
        bool IsEmailVerified
        string Role
        datetime CreatedAt
        datetime UpdatedAt
        timestamp RowVersion
    }
    
    Country {
        int Id PK
        string Code UK
        string Name
        string TimeZone
        bool IsActive
    }
    
    Package {
        int Id PK
        string Name
        int Credits
        decimal Price
        int CountryId FK
        int ValidityDays
        bool IsActive
        datetime CreatedAt
    }
    
    UserPackage {
        int Id PK
        int UserId FK
        int PackageId FK
        int RemainingCredits
        datetime PurchasedAt
        datetime ExpiresAt
        bool IsExpired
        string TransactionId
        timestamp RowVersion
    }
    
    ClassSchedule {
        int Id PK
        string Title
        string Description
        int CountryId FK
        datetime StartTime
        datetime EndTime
        int Capacity
        int AvailableSlots
        int RequiredCredits
        bool IsActive
        datetime CreatedAt
        datetime UpdatedAt
        timestamp RowVersion
    }
    
    Booking {
        int Id PK
        int UserId FK
        int ClassScheduleId FK
        int UserPackageId FK
        int CreditsUsed
        datetime BookedAt
        datetime CheckedInAt
        bool IsCancelled
        datetime CancelledAt
        bool IsRefunded
        string Status
        timestamp RowVersion
    }
    
    WaitlistEntry {
        int Id PK
        int UserId FK
        int ClassScheduleId FK
        datetime JoinedAt
        int Position
        bool IsPromoted
        datetime PromotedAt
        bool IsRefunded
    }
```

## Table Descriptions

### Users Table
Stores user accounts with authentication credentials and profile information.

**Key Indexes:**
- `idx_email` on Email (for login lookups)
- `idx_country` on CountryId (for regional queries)

### Countries Table
Reference data for supported countries/regions.

### Packages Table
Credit packages available for purchase in each country.

**Key Indexes:**
- `idx_country_active` on (CountryId, IsActive)

### UserPackages Table
User's purchased packages with remaining credits and expiry tracking.

**Key Indexes:**
- `idx_user_active` on (UserId, IsExpired, ExpiresAt)
- `idx_expiry` on (ExpiresAt, IsExpired) for cleanup jobs

### ClassSchedules Table
Available class sessions with capacity and timing.

**Key Indexes:**
- `idx_country_time` on (CountryId, StartTime, IsActive)
- `idx_start_time` on StartTime (for scheduling queries)

### Bookings Table
Confirmed bookings linking users to class schedules.

**Key Indexes:**
- `idx_user_class` on (UserId, ClassScheduleId)
- `idx_class_status` on (ClassScheduleId, Status)

### WaitlistEntries Table
Users waiting for slots in full classes (FIFO queue).

**Key Indexes:**
- `unique_user_class` on (UserId, ClassScheduleId)
- `idx_class_position` on (ClassScheduleId, Position, IsPromoted)

## Concurrency Control

### RowVersion (Timestamp)
Used for optimistic concurrency in:
- UserPackages (credit updates)
- ClassSchedules (slot updates)
- Bookings (status changes)

### Redis Keys
- `class:{id}:slots` - Available slot counter
- `class:{id}:waitlist` - Sorted set for FIFO waitlist