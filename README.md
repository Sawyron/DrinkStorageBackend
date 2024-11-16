# DrinkStorage

## Base url: host/api/v1/

# Resources

## Product

### / GET
- Response: product[]

### by-ids/ GET
- Parameters
  - ids: uuid[]
- Response: product[]

### search/
- Parameters
  - maxPrice: int
  - brandId: uuid | null
- Response: product[]

### import POST
Imports products from xlsx file
- Request body
  - Content-Type: multipart/form-data
  - parameters
    - file

## Order

### / POST
Places the order for specified items and return change for given coins
- Request body
  - type:
```json
{
    "orderItems": ["orderItem"],
    "coins": ["coin"]
}
```
- Response
```json
[
    {
        "id": "uuid",
        "value": "int",
        "quantity": "int"   
    }
]
```

# Data types
- product
```json
{
    "products": {
        "id": "uuid",
        "name": "string",
        "price": "int",
        "quantity": "int",
        "imageUlr": "string",
        "BrandId": "uuid"
    },
    "maxPrice": "int"
}
```
- orderItem
```json
{
    "productId": "uuid",
    "quantity": "int"
}
```
- coin
```json
{
    "id": "uuid",
    "quantity": "int"
}
```
