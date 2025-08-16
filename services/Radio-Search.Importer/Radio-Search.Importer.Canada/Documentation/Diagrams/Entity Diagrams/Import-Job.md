```mermaid
---
title: Full Import Job Tracking
---
erDiagram
    ImportJob {
        int ImportJobID PK
        enum Status
        enum CurrentStep
        DateTime StartTime
        DateTime EndTime
    }

    ImportJobChunkFile {
        int ImportJobID PK, FK
        int FileID PK
        enum Status
        DateTime StartTime
        DateTime EndTime
    }

    ImportJobStats {
        int ImportJobID PK, FK
    }


    LicenseRecord {
        string CanadaLicenseRecordID PK
        int Version PK
        bool IsValid
    }

    LicenseRecordHistory {
        int LicenseRecordHistoryId PK
        string CanadaLicenseRecordID FK
        int Version FK
        int ImportJobID
    }

    ImportJob ||--o{ ImportJobChunkFile : "Has Many Files"
    ImportJob ||--o{ LicenseRecordHistory : "Has Many Record Histories"
    ImportJob ||--|| ImportJobStats : "Has One Stats Record"
    LicenseRecord ||--|{ LicenseRecordHistory : "Has one or many Records"
```