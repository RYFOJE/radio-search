```mermaid
sequenceDiagram
    autonumber
    participant Blob as Blob Storage
    participant Importer as Canada Importer
    participant DB as Importer DB

    Blob ->> Importer: Trigger on new CSV chunk
    activate Importer

        Importer ->> Blob: Download Existing IDs CSV
        Blob -->> Importer: Existing IDs CSV

        activate Importer
            Importer ->> Importer: Identify new licenses
            Importer ->> Importer: Identify missing licenses in chunk
            Importer ->> DB: Query for unmatched rows
            DB -->> Importer: Unmatched rows
            Importer ->> Importer: Verify updated rows
        deactivate Importer

        Importer ->> Blob: Save processed results (New/Updated/Deleted)
    deactivate Importer
```