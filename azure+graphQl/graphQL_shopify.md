## Struttura stock prodotti

    Product (ProdottoColore)
    └─ variants(taglie) → ProductVariant
                    └─ inventoryItem → InventoryItem
                                └─ inventoryLevels → InventoryLevel
                                        ├─ location → Location
                                        └─ available (quantità)

    // le relaazioni sono bidirezionali
    Product
    ├─ collections → Collection (SmartCollection o CustomCollection)
    └─ catalogs → Catalog (tramite publication)

    Collection
    ├─ products → Product
    └─ metafields

    Catalog
    ├─ resources → Product, Collection, ecc.
    └─ market → Market

    // riassunto
    Product
    ├─ variants → ProductVariant → inventoryItem → InventoryLevel → Location
    ├─ options → ProductOption → optionValues
    ├─ metafields → Metafield
    ├─ collections → Collection
    └─ catalogs → Catalog

## Metafields
Un metafield è un campo personalizzato che può essere aggiunto a un oggetto per salvare informazioni extra che non esistono nei campi standard.

Struttura:
- namespace: Un "contenitore" logico (es: custom, logistica, marketing)
- key: Nome del campo (es: video_url, composizione)
- value: Il contenuto (es: "https://youtube.com/...")
- type: Tipo di dato (es: string, integer, boolean, json, ecc.)

Aggiungere un metafield:

    mutation {
    metafieldsSet(metafields: [
        {
        ownerId: "gid://shopify/Product/1234567890",  # ID del prodotto
        namespace: "custom",
        key: "materiale",
        type: "single_line_text_field",
        value: "100% cotone"
        }
    ]) {
        metafields {
        id
        key
        value
        }
        userErrors {
        field
        message
        }
    }
    }

Leggere un metafield:

    {
    product(id: "gid://shopify/Product/1234567890") {
        title
        metafield(namespace: "custom", key: "materiale") {
        id
        namespace
        key
        value
        type
        }
    }
    }
